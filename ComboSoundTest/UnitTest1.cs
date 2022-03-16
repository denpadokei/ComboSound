using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ComboSoundTest
{
    [TestClass]
    public class UnitTest1
    {
        private static readonly Regex _songFileRegex = new Regex(@"combo_[0-9]{4}\.wav");
        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(_songFileRegex.IsMatch("combo_0050.wav"), true);
            Assert.AreEqual(int.TryParse(Regex.Match("combo_0050.wav", "[0-9]{4}").Value, out var num), true);
            Assert.AreEqual(num, 50);
        }

        [TestMethod]
        public void EnumrateFiles()
        {
            foreach (var item in Directory.EnumerateFiles(@"D:\Oculus\Software\hyperbolic-magnetism-beat-saber\UserData\ComboSound", "*.wav").Where(x => _songFileRegex.IsMatch(Path.GetFileName(x)))) {
                Console.WriteLine(item);
            }
        }
    }
}
