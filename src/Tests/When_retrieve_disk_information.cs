namespace EnvAssertions.Tests
{
    using NUnit.Framework;
    using FluentAssertions;
    using EnvAssertions.Servers;

    public class When_retrieve_disk_information
    {
        private FileSize diskSize;

        [TestFixtureSetUp]
        public void Given_local_computer()
        {            
            var local = Server.Connect("localhost");            
            diskSize = local.Disks["C"].FreeSpace;
        }

        [Test]
        public void Should_report_non_zero_disk_size() 
        {
            diskSize.Should().NotBe(0);
        }
    }
}
