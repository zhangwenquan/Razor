namespace AspNetCore
{
    #line hidden
    using System;
    using System.Threading.Tasks;
#line 5 "TestFiles/IntegrationTests/CodeGenerationTests/RazorPages.cshtml"
using Microsoft.AspNetCore.Mvc.RazorPages;

#line default
#line hidden
    public class TestFiles_IntegrationTests_CodeGenerationTests_RazorPages_cshtml : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        private global::DivTagHelper __DivTagHelper = null;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(7, 2, true);
            EndContext();
            BeginContext(57, 2, true);
            EndContext();
            BeginContext(101, 4, true);
            EndContext();
            BeginContext(478, 78, true);
            EndContext();
            __DivTagHelper = CreateTagHelper<global::DivTagHelper>();
            BeginContext(556, 31, false);
            EndContext();
            BeginContext(587, 6, true);
            EndContext();
            BeginContext(617, 48, true);
            EndContext();
            BeginContext(666, 4, false);
#line 29 "TestFiles/IntegrationTests/CodeGenerationTests/RazorPages.cshtml"
                                         __o = Name;

#line default
#line hidden
            EndContext();
            BeginContext(670, 18, true);
            EndContext();
            BeginContext(711, 101, true);
            EndContext();
            __DivTagHelper = CreateTagHelper<global::DivTagHelper>();
            BeginContext(688, 130, false);
            EndContext();
            BeginContext(818, 6, true);
            EndContext();
            __DivTagHelper = CreateTagHelper<global::DivTagHelper>();
            BeginContext(593, 237, false);
            EndContext();
            BeginContext(830, 6, true);
            EndContext();
            BeginContext(860, 10, true);
            EndContext();
            BeginContext(909, 83, true);
            EndContext();
            __DivTagHelper = CreateTagHelper<global::DivTagHelper>();
            BeginContext(870, 128, false);
            EndContext();
            BeginContext(998, 6, true);
            EndContext();
            __DivTagHelper = CreateTagHelper<global::DivTagHelper>();
            BeginContext(836, 174, false);
            EndContext();
            BeginContext(1010, 11, true);
            EndContext();
        }
        #pragma warning restore 1998
#line 7 "TestFiles/IntegrationTests/CodeGenerationTests/RazorPages.cshtml"
            
    public class NewModel : PageModel
    {
        public IActionResult OnPost(Customer customer)
        {
            Name = customer.Name;
            return Redirect("~/customers/inlinepagemodels/");
        }

        public string Name { get; set; }
    }

    public class Customer
    {
        public string Name { get; set; }
    }

#line default
#line hidden
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<NewModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<NewModel>)PageContext?.ViewData;
        public NewModel Model => ViewData.Model;
    }
}
