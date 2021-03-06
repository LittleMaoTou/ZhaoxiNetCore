using Nest;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Zhaoxi.Helper;
using Zhaoxi.IocDI.Model;

namespace NUnitTestProject
{
    public class Tests
    {
        private ElasticSearchHelper elasticSearchHelper = new ElasticSearchHelper("http://localhost:9200");
        private ElasticSearchExtendHelper<user> elasticSearchExtend = new ElasticSearchExtendHelper<user>();

        /// <summary>
        /// 类似于构造函数
        /// </summary>
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var isexists = elasticSearchHelper.IsExistsByElasticClientIndex("zhaoxi");
            Assert.IsTrue(isexists);
            Assert.Pass();
        }

        [Test]
        public void Test2()
        {
            var isexists = elasticSearchHelper.CreateElasticClientIndex("zhaoxi1");
            Assert.IsTrue(isexists);
            Assert.Pass();
        }

        [Test]
        public void Test3()
        {
            var isexists = elasticSearchHelper.DeleteElasticClientIndex("zhaoxi1");
            Assert.IsTrue(isexists);
            Assert.Pass();
        }

        [Test]
        public async Task Test4()
        {
            user user = new user();
            user.Account = "111";
            user.Age = 19;
            user.Name = "z2";
            var re = await elasticSearchExtend.InsertEntityAsync(user);
            Assert.IsTrue(re);
            Assert.Pass();
        }
        [Test]
        public async Task Test5()
        {
            var re = await elasticSearchExtend.FindOne("5mix73IBv-C3Ucz42PR-");
            Assert.IsTrue(re != null);
            Assert.Pass();
        }

        [Test]
        public async Task Test6()
        {
            user user = new user();
            user.Name = "z4yh";
            var re = await elasticSearchExtend.UpdateEntityAsync("5mix73IBv-C3Ucz42PR-", user);
            Assert.IsTrue(re);
            Assert.Pass();
        }

        [Test]
        public async Task Test7()
        {
            var re = await elasticSearchExtend.FindAll();
            Assert.IsTrue(re.Count > 0);
            Assert.Pass();
        }

        [Test]
        public async Task Test8()
        {
            var re = await elasticSearchExtend.DeleteEntityAsync("5mix73IBv-C3Ucz42PR-");
            Assert.IsTrue(re);
            Assert.Pass();
        }

        [Test]
        public async Task Test9()
        {
            List<user> list = new List<user>();
            list.Add(new user() { Account = "520", Age = 22, Name = "dd" });
            list.Add(new user() { Account = "521", Age = 20, Name = "mm" });
            var re = await elasticSearchExtend.InsertManyEntityAsync(list);
            Assert.IsTrue(re);
            Assert.Pass();
        }

        [Test]
        public async Task Test10()
        {
            //两种方法都行
            //.Query(q => q.Match(m => m.Field(f => f.Name).Query("其他笔记本6"))) //单字段全文关键字检索 只要Name中包含值即可，且自带分词 
            //.Query(q => q.MultiMatch(m => m.Fields(fd=>fd.Fields(f=>f.Name,f=>f.OtherInfo)).Query("1神23456789"))) //多字段全文关键字检索 Name或OtherInfo包含该值即可，且自带分词 
            //.Analyzer("") // 该分词方法可不需要，因为上面的查询自带分词
            //.Query(q => q.Bool(b=>b.Must(m=>m.Term(p=>p.Field(f=>f.Id).Value(4)))))  //条件必须符合，无分词，有一些数据类型可能查询失败
            //.Query(q => q.Range(c => c.Field(f => f.Id).LessThanOrEquals(5).GreaterThanOrEquals(3))) //范围查询
            //.Sort(t => t.Ascending(p=>p.Id)) //id升序
            //.From(0) //分页 第几条开始展示
            //.Size(3) //分页，每页显示多少条

            //多个条件一起搜索，例子如下
            //var matchQuery = new List<Func<QueryContainerDescriptor<Computer>, QueryContainer>>
            //{
            //    must => must.Bool(b => b.Must(m => m.Term(p => p.Field(f => f.Id).Value(5)),
            //                                    m => m.Term(p => p.Field(f => f.Name).Value("神州笔记本1"))
            //                                 )
            //                     ),
            //    range => range.Range(c => c.Field(p => p.Id).LessThanOrEquals(5).GreaterThanOrEquals(3))
            //};
            //var tr = es.Search<Computer>(x=>x.Index("realyuseit").Query(q=>q.Bool(b=>b.Must(matchQuery))))


            //1. 
            //SearchDescriptor<user> searchDescriptor = new SearchDescriptor<user>();
            //searchDescriptor.From(0);
            //searchDescriptor.Size(10);
            //searchDescriptor.Query(q => q.Match(m => m.Field(f => f.Name).Query("mm")));
            //var list = await elasticSearchExtend.FindWhere(searchDescriptor);

            //2.
            var searchRequest = new SearchRequest<user>(Nest.Indices.All)
            {
                From = 0,
                Size = 10,
                Query = new MatchQuery
                {
                    Field = Infer.Field<user>(f => f.Name),
                    Query = "mm"
                }
            };
            var list = await elasticSearchExtend.FindWhere(searchRequest);
            Assert.Pass();
        }


        [Test]
        public void Test11()
        {
            //创建单个生产者 
            // RabbitMQProductionConsumer.CreateSingleProducer();

            //生产者接收到请求时Received事件
            //RabbitMQProductionConsumer.ReveicedProducerEvent();

            //多个生产者,多个线程同时开始
            //生产者01
            // Task.Run(() => { RabbitMQProductionConsumer.CreateMutiProducer(1); });
            //生产者02
            //Task.Run(() => { RabbitMQProductionConsumer.CreateMutiProducer(2); });

            //DirectExchange
            //Exchange.DirectExchangeProducer();
            //Exchange.DirectExchangeConsumerLogAll();
            //Exchange.DirectExchangeConsumerLogError();


            //FanoutExchange
            //Exchange.FanoutExchangeProducer();
            //Exchange.FanoutExchangeConsumer("Consumer001");
            //Exchange.FanoutExchangeConsumer("Consumer002");

            //Priority 优先级
            //PriorityQueue.PriorityProducer();
            //key要对应上
            //RabbitMQProductionConsumer.ReveicedProducerEventByConsumer();

            //消息确认 2种方式: 1. Tx事务模式 2. Confirm模式:
            //MessageAffirm.MessageTx();
            //MessageAffirm.MessageComfirm();

            //Assert.IsTrue(re);
            Assert.Pass();
        }


        [Test]
        public void Test12()
        {
            #region 开启线程
            //开启线程
            ThreadStart threadStart = () =>
            {
                Thread.Sleep(500);
                Console.WriteLine($"开启线程,线程id:" + Thread.CurrentThread.ManagedThreadId + ",时间:" + DateTime.Now);
            };
            Thread thread = new Thread(threadStart);
            thread.Start(); //开启

            //thread.Suspend();//挂起
            //thread.Resume();//恢复(不是实时挂起和恢复，有可能延迟，因为CPU分片)

            //thread.Abort();//终结线程并抛出异常
            //Thread.ResetAbort();//把终结的线程再次启用

            //thread.Priority = ThreadPriority.Highest;//优先级，提高倍率的优先级，并不绝对。

            //thread.IsBackground = true;//后台线程,进程结束，线程就结束,
            //thread.IsBackground = false;//前台线程，进程结束后，任务也结束，线程才接受

            //thread.ThreadState != ThreadState.Running;//线程状态

            //thread.Join(200);//限时等待，会阻塞，等待线程结束


            //ThreadPool.QueueUserWorkItem(o=>);

            #endregion
            Assert.Pass();
        }
    }
}