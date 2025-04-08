namespace InfoTrack.Tests.ServiceTests.Helpers;

public static class GoogleResponseHelper
{
    public static string GetGoogleHtmlWithMatches() =>
        @"
            <html>
            <body>
                <div class='g'>Result 1</div>
                <div class='g'>Result with <a href='https://www.infotrack.co.uk/landing'>www.infotrack.co.uk</a></div>
                <div class='g'>Result 3</div>
                <div class='g'>Result 4</div>
                <div class='g'>Result with <span>www.infotrack.co.uk</span> in text</div>
                <div class='g'>Result 6</div>
                <div class='g'>Result 7</div>
                <div class='g'>Result 8</div>
                <div class='g'>Result with reference to www.infotrack.co.uk/services</div>
            </body>
            </html>
        ";

    public static string GetGoogleHtmlWithNoMatches() =>
        @"
            <html>
            <body>
                <div class='g'>Result 1</div>
                <div class='g'>Result 2</div>
                <div class='g'>Result 3</div>
                <div class='g'>Result 4 with other domain www.othersite.com</div>
            </body>
            </html>
        ";
    
    public static string GetGoogleHtmlWithDifferentClassNames()
    {
        return @"
                <html>
                <body>
                    <div class='tF2Cxc'><a href='https://www.infotrack.co.uk'>www.infotrack.co.uk</a></div>
                    <div class='tF2Cxc'>Result 2</div>
                    <div class='tF2Cxc'>Result 3</div>
                    <div class='yuRUbf'>Result with <span>www.infotrack.co.uk</span></div>
                </body>
                </html>
            ";
    }

    public static string GetGoogleHtmlWithVariantDomains()
    {
        return @"
                <html>
                <body>
                    <div class='g'>Result 1</div>
                    <div class='g'>Result with <a href='https://infotrack.co.uk/landing'>infotrack.co.uk</a></div>
                    <div class='g'>Result 3</div>
                    <div class='g'>Result 4</div>
                    <div class='g'>Result with <span>www.infotrack.co.uk</span> in text</div>
                    <div class='g'>Result 6</div>
                    <div class='g'>Another result for <a href='http://uk.infotrack.co.uk'>uk.infotrack.co.uk</a></div>
                </body>
                </html>
            ";
    }
}