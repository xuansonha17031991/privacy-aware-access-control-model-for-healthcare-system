A. Requirements:

	1. Visual Studio 2017.
	2. MongoDB 4.0 or higher.
	3. Robo3t 1.2.1.
	4. Studio 3t.

B. Step-By-Step Instruction:

B.1 Setting up the data path for MongDB stores your Database:
- Install MongoDB (You should install MongoDB  to a partition which doesn't contain the OS) at the folder name "Sever"
- Create new folder and name it "Data"
- Go to "@yourpartition/Sever/bin" and run Command Prompt (cmd) in this folder and type 'mongod --dbpath "@yourpartition/Data"' (doesn't include '')
	For example: /Users/lotus/Desktop/MongoDB/Sever/bin>mongod --dbpath "/Users/lotus/Desktop/MongoDB/Data"
- You will see the notification "waiting for connections on port 27017". Then, you open Robo3T and connect to database. (DO NOT CLOSE CMD WINDOWS DURING PROCESS)

B.2 Create database and add collection:
- Create 2 databases 'Resource' and 'Policy'
- Create collections in 'Resource' database and insert documents.
- Create collections in 'Policy' database and insert documents.
Note: In 'Policy' you must have at least 2 collections. One for Access Control Policy and another for Privacy Policy.

B.3 Setting in sourcecode:
- Run VS 2017 and open Source Code.
- You need to setup 4 class :
a.> In "PrivacyABAC.UnitTest/TestConfiguration.cs", you must change 2 values: "PolicyDatabaseName" and "UserDatabaseName". 
"PolicyDatabaseName"'s value is name of database which contain your policy (for example: Policy).
"UserDatabaseName"'s value is name of database which contain your resource (for example: Resource).
b.> In "PrivacyABAC.MongoDb/Repository/AccessControlPolicyMongoDbRepository.cs". In this class you will find a variable named 'data' and change the name of
collection contain your Access Control Policy (for example: AccessControlPolicy).
c. In "PrivacyABAC.MongoDb/Repository/PrivacyPolicyMongoDbRepository.cs". In this class you will find a variable named 'data' and change the name of
collection contain your Privacy Policy (for example: PrivacyPolicy).
d.1 In "PrivacyABAC.UnitTest/PerformanceTest/PrivacyTest.cs". You will find the first string variable named 'collectionName'. This variable's value is the collection
contain your resource your need access (for example: Patient).
d.2 At variable 'user' is Subject infomation, you can change it to suitable for your policy.


B.4: Run testcase:
a.> Case 1 is run the access control policy "Subject's major is 'doctor' can see detail of patient". So you need to check 2 breakpoints, one in first line
of testcase and another in last line of testcase, then you right click and choose 'Debug test' to start (press F10 for skip to next line). Finally, you will get
a result in variable named 'result' in testcase.
b.> Case 2 is run both access control policy and privacy policy. Steps to perform like Case 1. Just notice, in this case we have 2 result: accessControlResult and 
privacyResult. If accessControlResult is null, you can't get privacyResult.



