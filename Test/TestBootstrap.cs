using Core;
using Test.Implements;

namespace Test
{
    [SetUpFixture]
    public class TestBootstrap
    {
        [OneTimeSetUp]
        public void GlobalSetup()
        {
            GroundhogContext.TaskInstanceLogic = new TaskInstanceLogic();
            GroundhogContext.TaskLogic = new TaskLogic();

            var languages = GroundhogContext.Languages;
            GroundhogContext.Language = GroundhogContext.LoadLanguage(GroundhogContext.DefaultLanguage);
        }
    }
}
