namespace InfoTrack.Tests.ServiceTests.Helpers;

public static class YahooResponseHelper
{
    public static string GetYahooHtmlWithMatches() =>
        @"
            <html>
            <body>
                <div class='Sr'>Result 1</div>
                <div class='Sr'>Result with <a href='https://www.infotrack.co.uk/page1'>www.infotrack.co.uk</a></div>
                <div class='Sr'>Result 3</div>
                <div class='Sr'>Another hit: www.infotrack.co.uk/tools</div>
                <div class='Sr'>www.infotrack.co.uk again here</div>
            </body>
            </html>
        ";

    public static string GetYahooHtmlWithNoMatches() =>
        @"
            <html>
            <body>
                <div class='Sr'>Result 1</div>
                <div class='Sr'>Result 2</div>
                <div class='Sr'>Other domain: www.notinfotrack.com</div>
            </body>
            </html>
        ";

    public static string GetYahooHtmlWithDifferentClassNames() =>
        @"
            <html>
            <body>
                <div class='dd algo'>www.infotrack.co.uk is here</div>
                <div class='dd algo'>Another</div>
                <li class='ov-a fz-16'>Hit again <a href='https://www.infotrack.co.uk'>www.infotrack.co.uk</a></li>
            </body>
            </html>
        ";

    public static string GetYahooHtmlWithVariantDomains() =>
        @"
            <html>
            <body>
                <div class='Sr'>Other site: www.somewhere.com</div>
                <div class='Sr'><a href='https://infotrack.co.uk/page'>infotrack.co.uk</a></div>
                <div class='Sr'><a href='https://uk.infotrack.co.uk'>uk.infotrack.co.uk</a></div>
                <div class='Sr'>www.infotrack.co.uk</div>
            </body>
            </html>
        ";
}