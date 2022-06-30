namespace KataPointOfSsale {
    public class Tests {

        private ScanCode scanCode;

        [SetUp]
        public void Setup() {
            scanCode = new ScanCode();
        }

        [Test]
        public void should_display_seven_with_twenty_five_when_send_12345() {
            var desiredResult = "$7.25";
            var result = scanCode.Decode("12345");
            Assert.AreEqual(desiredResult, result);
        }

        [Test]
        public void should_display_twelve_and_fifty_when_send_23456() {
            var desiredResult = "$12.50";
            var result = scanCode.Decode("23456");
            Assert.AreEqual(desiredResult, result);
        }
    }

    public class ScanCode {
        public string Decode(string code) {
            if (code.Equals("12345")) return "$7.25";
            return null;
        }
    }
}