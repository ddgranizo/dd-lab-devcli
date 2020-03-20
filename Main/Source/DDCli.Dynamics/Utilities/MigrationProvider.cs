using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDCli.Dynamics.Utilities
{
    public static class MigrationProvider
    {

        public static readonly string[] _excludedComparasionAttributes = { "createdon", "modifiedon", "createdby", "modifiedby", "ownerid", "owninguser",
                "owningbusinessunit", "createdonbehalfby", "modifiedonbehalfby" };
        public static readonly string[] _stateAttributes = { "statecode", "statuscode" };

        public static void Migrate(Action<string> loggerHandler, IOrganizationService from, IOrganizationService to, string[] entities)
        {
            int counter = 1;
            int completedOperations = 0;
            var timer = new Stopwatch();
            timer.Start();
            do
            {
                loggerHandler($"Trying round {counter}...");
                completedOperations = DoCloneRound(loggerHandler, from, to, entities);
            } while (completedOperations > 0);

            TimeSpan t = TimeSpan.FromMilliseconds(timer.ElapsedMilliseconds);
            string answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
                                    t.Hours,
                                    t.Minutes,
                                    t.Seconds,
                                    t.Milliseconds);
            loggerHandler($"Migration completed in {answer}");
        }

        private static int DoCloneRound(Action<string> loggerHandler, IOrganizationService from, IOrganizationService to, string[] entities)
        {

            throw new NotImplementedException();
            //Todavia no podemos saber si una entidad es entidad o interseccion

            var createdCounterAll = 0;
            var updatedCounterAll = 0;
            var associatedCounterAll = 0;
            var disassociatedCounterAll = 0;
            var deletedCounterAll = 0;
            var errorCounterAll = 0;
            foreach (var entity in entities)
            {
                var createdCounter = 0;
                var updatedCounter = 0;
                var deletedCounter = 0;
                var entityTrimed = entity.Trim();
                var errorCounter = 0;
                loggerHandler($"Checking entity '{entityTrimed}'...");
                var fromRecords = RetrieveAllRecords(from, entityTrimed);
                var toRecords = RetrieveAllRecords(to, entityTrimed);
                foreach (var fromRecord in fromRecords)
                {
                    var isRecordInDestionationWithSameId = IsRecordWithSameId(fromRecord.Id, toRecords);
                    if (!isRecordInDestionationWithSameId)
                    {
                        try
                        {
                            CreateEntity(to, fromRecord);
                            loggerHandler($"\tCreated record {++createdCounter} with Id={fromRecord.Id}");
                        }
                        catch (Exception ex)
                        {
                            errorCounter++;
                            loggerHandler($"\tError creating record {++createdCounter} with Id={fromRecord.Id}. Error: {ex.Message}");
                        }
                    }
                }

                foreach (var toRecord in toRecords)
                {
                    var isRecordInSourceWithSameId = IsRecordWithSameId(toRecord.Id, fromRecords);
                    if (!isRecordInSourceWithSameId)
                    {
                        try
                        {
                            DeleteEntity(to, toRecord);
                            loggerHandler($"\tDeleted record {++deletedCounter} with Id={toRecord.Id}");
                        }
                        catch (Exception ex)
                        {
                            errorCounter++;
                            loggerHandler($"\tError deleting record {++deletedCounter} with Id={toRecord.Id}");
                        }
                    }
                }

                toRecords = RetrieveAllRecords(to, entityTrimed);
                foreach (var fromRecord in fromRecords)
                {
                    var recordWithSameId = toRecords.FirstOrDefault(k => k.Id == fromRecord.Id);
                    if (recordWithSameId != null)
                    {
                        var differentAttributes = GetEntityDifferentAttributes(fromRecord, recordWithSameId);
                        if (differentAttributes.Count > 0)
                        {
                            try
                            {
                                UpdateEntity(to, fromRecord, differentAttributes);
                                loggerHandler($"\tUpdated record {++updatedCounter} with {differentAttributes.Count} attributes with Id={fromRecord.Id}");
                            }
                            catch (Exception ex)
                            {
                                errorCounter++;
                                loggerHandler($"\tError updating record {++updatedCounter} with {differentAttributes.Count} attributes with Id={fromRecord.Id}. Error: {ex.Message}");
                            }
                        }
                    }
                    else
                    {
                        errorCounter++;
                        loggerHandler($"\tError. Record with Id={fromRecord.Id} should be already created in destination");
                    }
                }
                createdCounterAll += createdCounter;
                updatedCounterAll += updatedCounter;
                deletedCounterAll += deletedCounter;
                errorCounterAll += errorCounter;
            }

            loggerHandler($"\tRound information:");
            loggerHandler($"\t\tCreated records: {createdCounterAll}");
            loggerHandler($"\t\tUpdated records: {updatedCounterAll}");
            loggerHandler($"\t\tDeleted records: {deletedCounterAll}");
            loggerHandler($"\t\tAssociated records: {associatedCounterAll}");
            loggerHandler($"\t\tDisassociated records: {disassociatedCounterAll}");
            loggerHandler($"\t\tError records: {errorCounterAll}");

            var totalOperations = 
                createdCounterAll + 
                updatedCounterAll + 
                deletedCounterAll + 
                associatedCounterAll + 
                disassociatedCounterAll + 
                errorCounterAll;

            loggerHandler($"\t\tTotal operations records: {totalOperations}");
            return totalOperations;
        }

        public static void Disassociate(IOrganizationService service,
            string schemaName,
            string firstEntity,
            Guid firstEntityId,
            string secondEntity,
            Guid secondEntityId)
        {
            service.Disassociate(firstEntity, firstEntityId, new Relationship(schemaName),
                new EntityReferenceCollection() { { new EntityReference(secondEntity, secondEntityId) } });
        }

        public static void Associate(IOrganizationService service,
            string schemaName,
            string firstEntity,
            Guid firstEntityId,
            string secondEntity,
            Guid secondEntityId)
        {
            service.Associate(firstEntity, firstEntityId, new Relationship(schemaName),
                new EntityReferenceCollection() { { new EntityReference(secondEntity, secondEntityId) } });
        }

        public static void UpdateEntity(IOrganizationService service, Entity entity, List<string> attributes)
        {
            var newEntity = new Entity(entity.LogicalName, entity.Id);
            foreach (var item in attributes)
            {
                newEntity[item] = entity.Attributes[item];
            }
            service.Update(newEntity);
        }

        public static void DeleteEntity(IOrganizationService service, Entity entity)
        {
            service.Delete(entity.LogicalName, entity.Id);
        }

        public static void CreateEntity(IOrganizationService service, Entity entity)
        {

            var newEntity = new Entity(entity.LogicalName, entity.Id);
            foreach (var item in entity.Attributes)
            {
                if (_excludedComparasionAttributes.ToList().IndexOf(item.Key) == -1
                    && _stateAttributes.ToList().IndexOf(item.Key) == -1)
                {
                    newEntity[item.Key] = item.Value;
                }
            }
            service.Create(newEntity);

            var stateEntity = new Entity(entity.LogicalName, entity.Id);
            foreach (var item in _stateAttributes)
            {
                stateEntity[item] = entity[item];
            }
            service.Update(stateEntity);
        }

        private static ManyToManyRelationshipMetadata GetRelationshipMetadata(IOrganizationService service, string entity)
        {
            return (ManyToManyRelationshipMetadata)
                ((RetrieveRelationshipResponse)service.Execute(new RetrieveRelationshipRequest()
                {
                    RetrieveAsIfPublished = true,
                    Name = entity,

                })).RelationshipMetadata;
        }

        public static List<Entity> RetrieveAllRecords(IOrganizationService service, string entity)
        {
            var moreRecords = false;
            int page = 1;
            var cookie = string.Empty;
            List<Entity> entities = new List<Entity>();
            do
            {
                var collection = service.RetrieveMultiple(new QueryExpression(entity)
                {
                    ColumnSet = new ColumnSet(true),
                    PageInfo = new PagingInfo()
                    {
                        PagingCookie = cookie,
                        Count = 5000,
                        PageNumber = page,
                    }
                });

                if (collection.Entities.Count >= 0) entities.AddRange(collection.Entities);
                moreRecords = collection.MoreRecords;
                if (moreRecords)
                {
                    page++;
                    cookie = string.Format("paging-cookie='{0}' page='{1}'", System.Security.SecurityElement.Escape(collection.PagingCookie), page);
                }
            } while (moreRecords);

            return entities;
        }


        private static bool IsRecordWithSameIntersection(
            Entity e,
            List<Entity> collection,
            string firstAttribute,
            string secondAttribute)
        {
            return collection.Count > 0
                && collection.Any(k => (Guid)k.Attributes[firstAttribute] == (Guid)e.Attributes[firstAttribute]
                                        && (Guid)k.Attributes[secondAttribute] == (Guid)e.Attributes[secondAttribute]);
        }

        private static bool IsRecordWithSameId(Guid id, List<Entity> collection)
        {
            return collection.Count > 0
                && collection.Any(k => k.Id == id);
        }


        private static List<string> GetEntityDifferentAttributes(Entity entity1, Entity entity2)
        {
            bool areEquals = true;
            var notEqualsAttributes = new List<string>();
            foreach (var item in entity1.Attributes)
            {
                if (_excludedComparasionAttributes.ToList().IndexOf(item.Key) == -1)
                {
                    var loopAreEquals = true;
                    var isSameAttributeInOther = entity2.Attributes.ContainsKey(item.Key);
                    if (!isSameAttributeInOther)
                    {
                        loopAreEquals = false;
                    }
                    else
                    {
                        var otherAtrribute = entity2.Attributes[item.Key];
                        loopAreEquals = AttribureAreEquals(item.Value, otherAtrribute);
                    }
                    if (!loopAreEquals)
                    {
                        notEqualsAttributes.Add(item.Key);
                    }
                    areEquals = !areEquals ? areEquals : loopAreEquals;
                }
            }
            if (!areEquals)
            {

            }
            return notEqualsAttributes;
        }

        private static bool AttribureAreEquals(object attribute1, object attribute2)
        {
            if (attribute1 == null && attribute2 != null)
            {
                return false;
            }
            if (attribute2 == null && attribute1 != null)
            {
                return false;
            }
            if (attribute1 is EntityReference && attribute2 is EntityReference)
            {
                return ((EntityReference)attribute1).Id == ((EntityReference)attribute2).Id;
            }
            else if (attribute1 is OptionSetValue && attribute2 is OptionSetValue)
            {
                return ((OptionSetValue)attribute1).Value == ((OptionSetValue)attribute2).Value;
            }
            else if (attribute1 is Money && attribute2 is Money)
            {
                return ((Money)attribute1).Value == ((Money)attribute2).Value;
            }
            else if (attribute1 is Guid && attribute2 is Guid)
            {
                return ((Guid)attribute1).Equals((Guid)attribute2);
            }
            else if (attribute1 is string && attribute2 is string)
            {
                return ((string)attribute1).Equals((string)attribute2);
            }
            else if (attribute1 is bool && attribute2 is bool)
            {
                return ((bool)attribute1).Equals((bool)attribute2);
            }
            else if (attribute1 is decimal && attribute2 is decimal)
            {
                return ((decimal)attribute1).Equals((decimal)attribute2);
            }
            else if (attribute1 is int && attribute2 is int)
            {
                return ((int)attribute1).Equals((int)attribute2);
            }
            else if (attribute1 is Int32 && attribute2 is Int32)
            {
                return ((Int32)attribute1).Equals((Int32)attribute2);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

    }
}
