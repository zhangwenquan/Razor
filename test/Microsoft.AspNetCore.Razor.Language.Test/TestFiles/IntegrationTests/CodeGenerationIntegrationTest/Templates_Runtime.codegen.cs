#pragma checksum "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8a16b856f52a2ed469347984184b8ac48080234f"
namespace Microsoft.AspNetCore.Razor.Language.IntegrationTests.TestFiles
{
    #line hidden
    public class TestFiles_IntegrationTests_CodeGenerationIntegrationTest_Templates_Runtime
    {
        #pragma warning disable 1998
        public async System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#line 11 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
  
    Func<dynamic, object> foo = 

#line default
#line hidden
            item => new Template(async(__razor_template_writer) => {
                PushWriter(__razor_template_writer);
                WriteLiteral("This works ");
#line 12 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
                                             Write(item);

#line default
#line hidden
                WriteLiteral("!");
                PopWriter();
            }
            )
#line 12 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
                                                               ;
    

#line default
#line hidden
#line 13 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
Write(foo(""));

#line default
#line hidden
            WriteLiteral("\r\n");
#line 16 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
   
    Func<dynamic, object> bar = 

#line default
#line hidden
            item => new Template(async(__razor_template_writer) => {
                PushWriter(__razor_template_writer);
                WriteLiteral("<p");
                BeginWriteAttribute("class", " class=\"", 411, "\"", 424, 1);
#line 17 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
WriteAttributeValue("", 419, item, 419, 5, false);

#line default
#line hidden
                EndWriteAttribute();
                WriteLiteral(">Hello</p>");
                PopWriter();
            }
            )
#line 17 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
                                                           ;
    

#line default
#line hidden
#line 18 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
Write(bar("myclass"));

#line default
#line hidden
            WriteLiteral("\r\n<ul>\r\n");
#line 22 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
Write(Repeat(10, item => new Template(async(__razor_template_writer) => {
    PushWriter(__razor_template_writer);
    WriteLiteral("<li>Item #");
#line 22 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
                   Write(item);

#line default
#line hidden
    WriteLiteral("</li>");
    PopWriter();
}
)));

#line default
#line hidden
            WriteLiteral("\r\n</ul>\r\n\r\n<p>\r\n");
#line 26 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
Write(Repeat(10,
    item => new Template(async(__razor_template_writer) => {
    PushWriter(__razor_template_writer);
    WriteLiteral(" This is line#");
#line 27 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
               Write(item);

#line default
#line hidden
    WriteLiteral(" of markup<br/>\r\n");
    PopWriter();
}
)));

#line default
#line hidden
            WriteLiteral("\r\n</p>\r\n\r\n<p>\r\n");
#line 32 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
Write(Repeat(10,
    item => new Template(async(__razor_template_writer) => {
    PushWriter(__razor_template_writer);
    WriteLiteral(": This is line#");
#line 33 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
                Write(item);

#line default
#line hidden
    WriteLiteral(" of markup<br />\r\n");
    PopWriter();
}
)));

#line default
#line hidden
            WriteLiteral("\r\n</p>\r\n\r\n<p>\r\n");
#line 38 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
Write(Repeat(10,
    item => new Template(async(__razor_template_writer) => {
    PushWriter(__razor_template_writer);
    WriteLiteral(":: This is line#");
#line 39 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
                 Write(item);

#line default
#line hidden
    WriteLiteral(" of markup<br />\r\n");
    PopWriter();
}
)));

#line default
#line hidden
            WriteLiteral("\r\n</p>\r\n\r\n\r\n<ul>\r\n    ");
#line 45 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
Write(Repeat(10, item => new Template(async(__razor_template_writer) => {
    PushWriter(__razor_template_writer);
    WriteLiteral("<li>\r\n        Item #");
#line 46 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
         Write(item);

#line default
#line hidden
    WriteLiteral("\r\n");
#line 47 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
          var parent = item;

#line default
#line hidden
    WriteLiteral("        <ul>\r\n            <li>Child Items... ?</li>\r\n");
    WriteLiteral("        </ul>\r\n    </li>");
    PopWriter();
}
)));

#line default
#line hidden
            WriteLiteral("\r\n</ul> ");
        }
        #pragma warning restore 1998
#line 1 "TestFiles/IntegrationTests/CodeGenerationIntegrationTest/Templates.cshtml"
            
    public HelperResult Repeat(int times, Func<int, object> template) {
        return new HelperResult((writer) => {
            for(int i = 0; i < times; i++) {
                ((HelperResult)template(i)).WriteTo(writer);
            }
        });
    }

#line default
#line hidden
    }
}
