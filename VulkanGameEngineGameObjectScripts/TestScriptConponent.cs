namespace VulkanGameEngineGameObjectScripts
{
    public class SimpleTest
    {
        public int counter = 0;

        public SimpleTest()
        {
            counter = 0;
        }

        public int SimpleFunction(int input)
        {
            counter++;
            return counter;
        }

        public void SimpleDestroy()
        {
            Console.WriteLine("Simple function called.");
        }
    }
}