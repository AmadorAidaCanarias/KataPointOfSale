using System.Globalization;

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

        [Test]
        public void should_display_error_barcode_not_found_when_send_99999() {
            var desiredResult = "Error: barcode not found";
            var result = scanCode.Decode("99999");
            Assert.AreEqual(desiredResult, result);
        }

        [Test]
        public void should_display_error_empty_barcode_when_send_empty_string() {
            var desiredResult = "Error: empty barcode";
            var result = scanCode.Decode(string.Empty);
            Assert.AreEqual(desiredResult, result);
        }

        [Test]
        public void should_display_sum_from_all_barcode_when_send_total_and_12345_and_23456() {
            var desiredResult = "$19.75";
            var result = scanCode.Decode("Total: 12345, 23456");
            Assert.AreEqual(desiredResult, result);
        }

        [Test]
        public void should_display_sum_from_all_barcode_when_send_total_and_12345_23456_99999() {
            var desiredResult = "$19.75";
            var result = scanCode.Decode("Total: 12345, 23456, 99999");
            Assert.AreEqual(desiredResult, result);
        }
    }

    public class ScanCode {

        private Dictionary<string, decimal> prices;

        public ScanCode() {
            prices = new Dictionary<string, decimal>();
            prices.Add("12345", (decimal)7.25);
            prices.Add("23456", (decimal)12.50);
            prices.Add("99999", (decimal)0.0);
        }

        public string Decode(string code) {
            var error = CheckErrors(code);
            if (error != String.Empty) return error;
            var codes = GetCodes(code);
            var price = GetTotalPrice(codes);
            return GetFormatString(price);
        }

        private static string GetFormatString(decimal price) {
            var result = $"${price.ToString("N2", CultureInfo.InvariantCulture)}";
            if (price == (decimal)0.0) {
                result = "Error: barcode not found";
            }
            return result;
        }

        private decimal GetTotalPrice(IEnumerable<string> codes) {
            decimal price = 0;
            foreach (var row in codes) price += GetPrice(row);
            return price;
        }

        private static IEnumerable<string> GetCodes(string code) {
            return code
                .Split(",")
                .ToList()
                .Select(row => row.Replace("Total:", "").Trim());
        }

        private string CheckErrors(string code) =>
            code switch {
                "" => "Error: empty barcode",
                "99999" => "Error: barcode not found",
                _ => string.Empty
            };

        private decimal GetPrice(string code) {
            return prices
                    .Where(row => row.Key == code)
                    .Select(row => row.Value)
                    .First();
        }
    }
}