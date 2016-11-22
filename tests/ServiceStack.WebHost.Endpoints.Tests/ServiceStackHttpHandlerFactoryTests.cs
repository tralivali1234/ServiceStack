using System;
using System.Collections.Generic;
using NUnit.Framework;
using ServiceStack.Host.Handlers;
using ServiceStack.Metadata;
using ServiceStack.Testing;
using ServiceStack.Text;

namespace ServiceStack.WebHost.Endpoints.Tests
{
    [TestFixture]
    public class ServiceStackHttpHandlerFactoryTests
    {
        ServiceStackHost appHost;

        [OneTimeSetUp]
        public void TestFixtureSetUp()
        {
            appHost = new BasicAppHost(GetType().GetAssembly()).Init();
            HostContext.CatchAllHandlers.Add(new PredefinedRoutesFeature().ProcessRequest);
            HostContext.CatchAllHandlers.Add(new MetadataFeature().ProcessRequest);
        }

        [OneTimeTearDown]
        public void TestFixtureTearDown()
        {
            appHost.Dispose();
        }

        readonly Dictionary<string, Type> pathInfoMap = new Dictionary<string, Type>
		{
            {"Metadata", typeof(IndexMetadataHandler)},
#if !NETCORE            
            {"Soap11", typeof(Soap11MessageReplyHttpHandler)},
            {"Soap12", typeof(Soap12MessageReplyHttpHandler)},
#endif

            {"Json/Reply", typeof(JsonReplyHandler)},
            {"Json/OneWay", typeof(JsonOneWayHandler)},
            {"Json/Metadata", typeof(JsonMetadataHandler)},

            {"Xml/Reply", typeof(XmlReplyHandler)},
            {"Xml/OneWay", typeof(XmlOneWayHandler)},
            {"Xml/Metadata", typeof(XmlMetadataHandler)},

            {"Jsv/Reply", typeof(JsvReplyHandler)},
            {"Jsv/OneWay", typeof(JsvOneWayHandler)},
            {"Jsv/Metadata", typeof(JsvMetadataHandler)},

#if !NETCORE
			{"Soap11/Wsdl", typeof(Soap11WsdlMetadataHandler)},
			{"Soap11/Metadata", typeof(Soap11MetadataHandler)},

			{"Soap12/Wsdl", typeof(Soap12WsdlMetadataHandler)},
			{"Soap12/Metadata", typeof(Soap12MetadataHandler)},
#endif
		};

        [Test]
        public void Resolves_the_right_handler_for_expexted_paths()
        {
            foreach (var item in pathInfoMap)
            {
                var expectedType = item.Value;
                var handler = HttpHandlerFactory.GetHandlerForPathInfo(null, item.Key, null, null);
                Assert.That(handler.GetType(), Is.EqualTo(expectedType));
            }
        }

        [Test]
        public void Resolves_the_right_handler_for_case_insensitive_expexted_paths()
        {
            foreach (var item in pathInfoMap)
            {
                var expectedType = item.Value;
                var lowerPathInfo = item.Key.ToLower();
                lowerPathInfo.Print();
                var handler = HttpHandlerFactory.GetHandlerForPathInfo(null, lowerPathInfo, null, null);
                Assert.That(handler.GetType(), Is.EqualTo(expectedType));
            }
        }
    }
}