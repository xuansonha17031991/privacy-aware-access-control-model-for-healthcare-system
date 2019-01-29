using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrivacyABAC.DbInterfaces.Repository;
using PrivacyABAC.MongoDb.Repository;
using PrivacyABAC.Core.Service;
using Microsoft.Extensions.Logging;
using PrivacyABAC.MongoDb;
using Newtonsoft.Json.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using PrivacyABAC.Core.Model;
using System.IO;
using System.Diagnostics;

namespace PrivacyABAC.UnitTest.PerformanceTest
{
    /// <summary>
    /// Access control policy 1: Subject's major is 'doctor' or 'nurse' can see detail of patient (Request is Doctor, read, patient)
    /// Privacy policy 1: Only 'doctor' can see ALL deltail of patient, 'nurse' only see Name_patient and first three number of phone_patient (example: 012xxxxxx)
    /// </summary>
    [TestClass]
    public class PrivacyTest
    {
        private string collectionName = "Patient";
        /// <summary>
        /// Case 1: AC1 
        /// </summary>
        [TestMethod]
        public void FirstCaseTest()
        {
            var container = TestConfiguration.GetContainer();
            var subjectRepository = container.Resolve<ISubjectRepository>();
            var resourceRepository = container.Resolve<IResourceRepository>();
            var service = container.Resolve<AccessControlService>();

            var user = JObject.Parse(@"
                {'name_patient': 'Loydie', 'major': 'doctor'}
            ");
            var environment = JObject.Parse("{}");
            var builder = Builders<BsonDocument>.Filter;
            
            var resource = resourceRepository.GetCollectionDataWithCustomFilter(collectionName, null);
            var action = "read";

            var subject = new Subject(user);
            var data = new Resource(resource, collectionName);
            var env = new EnvironmentObject(environment);

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = service.ExecuteProcess(subject, data, action, env);
            stopwatch.Stop();
            File.WriteAllText(@"D:\log_test\first_case.txt", stopwatch.ElapsedMilliseconds.ToString() + " " + result.Data.Count);
        }

        
        /// <summary>
        /// Case 2: AC1 + PP1 
        /// </summary>
        [TestMethod]
        public void SecondCaseTest()
        {
            var container = TestConfiguration.GetContainer();
            var subjectRepository = container.Resolve<ISubjectRepository>();
            var resourceRepository = container.Resolve<IResourceRepository>();
            var accessControlService = container.Resolve<AccessControlService>();
            var privacyService = container.Resolve<PrivacyService>();
            
            var user = JObject.Parse(@"
                {'name_patient': 'Loydie', 'major': 'nurse'}
            ");
            var environment = JObject.Parse("{}");
            var builder = Builders<BsonDocument>.Filter;
            var resource = resourceRepository.GetCollectionDataWithCustomFilter(collectionName, null);
            var action = "read";

            var subject = new Subject(user);
            var data = new Resource(resource, collectionName);
            var env = new EnvironmentObject(environment);


            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var accessControlResult = accessControlService.ExecuteProcess(subject, data, action, env);
            var data2 = new Resource(accessControlResult.Data.ToArray(), collectionName);
            var privacyResult = privacyService.ExecuteProcess(subject, data2, action, env);
            stopwatch.Stop();
            File.WriteAllText($@"C:\Users\ttqnguyet\Downloads\test.txt", stopwatch.ElapsedMilliseconds.ToString() + " " + data2.Data.ElementAt(0).ToString());
            
        }

    }
}
