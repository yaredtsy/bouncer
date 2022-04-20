using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
// using UnityEngine.Assertions;
namespace Tests
{
    public class NewTestScript
    {

        [UnityTest]
        public IEnumerator WinTest()
        {

            SceneManager.LoadScene("game", LoadSceneMode.Single);
            var gameManger = (GameObject)null;

            yield return new WaitUntil(() => (gameManger = GameObject.Find("GameManger")) != null);
            GameManger manger = gameManger.GetComponent<GameManger>();

            Assert.IsNotNull(manger);
            Assert.IsTrue(manger.LoadLevel(manger.winPrefab));
            yield return new WaitForSeconds(0.4f);
            manger.OnPlayTap();

            yield return new WaitForSeconds(1);

            Assert.IsTrue(manger.win);

            yield return new WaitForSeconds(1);
        }
        [UnityTest]
        public IEnumerator LossTest()
        {
            SceneManager.LoadScene("game", LoadSceneMode.Single);
            var gameManger = (GameObject)null;

            yield return new WaitUntil(() => (gameManger = GameObject.Find("GameManger")) != null);
            GameManger manger = gameManger.GetComponent<GameManger>();

            Assert.IsNotNull(manger);
            Assert.IsTrue(manger.LoadLevel(manger.lossPrefab));
            yield return new WaitForSeconds(0.4f);
            manger.OnPlayTap();

            yield return new WaitForSeconds(1);

            Assert.IsFalse(manger.win);

            yield return new WaitForSeconds(1);
        }

        public class DataStruct
        {
            public Vector3 start;
            public Vector3 endpos;
            public DataStruct(Vector3 start, Vector3 endpos)
            {
                this.start = start;
                this.endpos = endpos;
            }
        }

        static DataStruct[] datas = {
            new DataStruct(new Vector3(-0.25f, 2, -1f), new Vector3(0.25f, 2, -1f)),
            new DataStruct(new Vector3(-0.75f,1f,-1f),  new Vector3(0.75f,1f,-1f)),
            new DataStruct(new Vector3(-1.25f,0,-1),    new Vector3(1.25f,0,-1)),
            new DataStruct(new Vector3(-2.65f,-1,-1),   new Vector3(2.65f,-1,-1))
            };

        [UnityTest]
        public IEnumerator LineDrawTest()
        {
            SceneManager.LoadScene("game", LoadSceneMode.Single);
            var drawManger = (GameObject)null;

            yield return new WaitUntil(() => (drawManger = GameObject.Find("DrawManger")) != null);
            DrawManger draw = drawManger.GetComponent<DrawManger>();
            foreach(DataStruct data in datas){
                Vector3 start = data.start; Vector3 end = data.endpos;


                float line = Vector3.Distance(start, end);

                yield return new WaitForSeconds(.5f);

                draw.StartMouseDown(start);
                draw.StartMouseDrag(end);

                draw.SetUpLine(line, start, end);

                Linesbehaviour.LineData linebehave = draw.linesbehaviour.GetLineData(line);

                Assert.AreEqual(linebehave.strength,draw.Col.sharedMaterial.bounciness,"Stength is not equal");
                Assert.IsTrue(linebehave.start<line,"Line length is less than");
                Assert.IsTrue(linebehave.end >line,"line Length is greater than end");

                Assert.IsNotNull(draw.Linerenderer);

                Assert.AreEqual(draw.Linerenderer.material.color,linebehave.color,"Colors Arent equal");
                Assert.AreEqual(draw.Linerenderer.startWidth,draw.Linerenderer.endWidth,"");
                Assert.AreEqual(draw.Linerenderer.startWidth,linebehave.width);

                Debug.Log("linebehave.star "+linebehave.start+" "+line+" "+draw.linesbehaviour.lineDatas.IndexOf(linebehave));

                yield return new WaitForSeconds(.5f);
            }

        }


    }
}
