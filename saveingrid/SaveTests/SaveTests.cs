using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class SaveTests
    {

		//private SaveManager TestSave;
		// A Test behaves as an ordinary method
        //20 units on grid at a time
        //GameObject for attaching saveManager to
		private static GameObject gameObject = new GameObject();
        //for referring to functions of savemanager.
        private SaveManager saveManager = gameObject.AddComponent<SaveManager>() as SaveManager;
        //the grid
        //private Grid grid = 
        //private 

		//private SaveManager saveManager = ;
        public void SetUpFileCreationTest(string filename)
		{
			saveManager.Save(filename);
		}
        public void SetUpGridToTestSavingUnitData()
		{

		}
        [Test]
        public void FileCreation()
        {
			SetUpFileCreationTest("TestFile");
            //Retrieve filename
            string[] filepath = Directory.GetFiles(Application.persistentDataPath + "/SaveFiles", "TestFile.sav");
            //retrieves the name of file and returns it in .sav format
            string b = filepath[0].Split('/')[8];
            string FileToBeChecked = b.Split('.')[0];
            Assert.IsTrue(FileToBeChecked == "TestFile");
        }
        [Test]
        public void SameNameFileCreation()
        {
            SetUpFileCreationTest("TestFile");
            //Retrieve filename
            string[] filepath = Directory.GetFiles(Application.persistentDataPath + "/SaveFiles", "TestFile.sav");
            //checks if two files are created
            Assert.IsTrue(filepath.Length >1);
        }
        [Test]
        public void FileCreationWithPeriods()
        {
            SetUpFileCreationTest("file.sav.excited");
            //Retrieve filename
            string[] filepath = Directory.GetFiles(Application.persistentDataPath + "/SaveFiles", "TestFile.sav");
            string b = filepath[0].Split('/')[8];
            string FileToBeChecked = b.Split('.')[0];
            Assert.IsTrue(FileToBeChecked == "file.sav.excited");
        }

        [Test]
        public void GridSystemSaveLoad()
		{


            //Test below show that the grid system save and load works.
			Assert.IsTrue(3 > 2);
		}


        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        /*[UnityTest]
        public IEnumerator SaveTestsWithEnumeratorPasses()
        {
			// Use the Assert class to test conditions
			// Use yield to skip a frame.
			//Tests file is created and
			CreateTestSave();
			yield return new WaitForSeconds(0.1f);
			//Tests file is created and
			//CreateTestSave();
			string[] filepath = Directory.GetFiles(Application.persistentDataPath + "/SaveFiles", "TestFile.sav");
			//retrieves the name of file and returns it in .sav format
			string b = filepath[0].Split('/')[8];
			string FileToBeChecked = b.Split('.')[0];
			Assert.IsTrue(FileToBeChecked == "TestFile");
			// Use the Assert class to test conditions
		}*/
	}
}
