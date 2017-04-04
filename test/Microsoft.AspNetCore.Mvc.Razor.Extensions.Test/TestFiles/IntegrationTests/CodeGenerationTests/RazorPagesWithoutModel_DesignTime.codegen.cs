namespace AspNetCore
{
    #line hidden
    using System;
    using System.Threading.Tasks;
#line 4 "TestFiles/IntegrationTests/CodeGenerationTests/RazorPagesWithoutModel.cshtml"
using Microsoft.AspNetCore.Mvc.RazorPages;

#line default
#line hidden
    public class TestFiles_IntegrationTests_CodeGenerationTests_RazorPagesWithoutModel_cshtml : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        private global::DivTagHelper __DivTagHelper = null;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(7, 2, true);
            EndContext();
            BeginContext(40, 2, true);
            EndContext();
            BeginContext(84, 4, true);
            EndContext();
            BeginContext(384, 77, true);
            EndContext();
            __DivTagHelper = CreateTagHelper<global::DivTagHelper>();
            BeginContext(461, 31, false);
            EndContext();
            BeginContext(492, 6, true);
            EndContext();
            BeginContext(522, 48, true);
            EndContext();
            BeginContext(571, 4, false);
#line 25 "TestFiles/IntegrationTests/CodeGenerationTests/RazorPagesWithoutModel.cshtml"
                                         __o = Name;

#line default
#line hidden
            EndContext();
            BeginContext(575, 18, true);
            EndContext();
            BeginContext(616, 101, true);
            EndContext();
            __DivTagHelper = CreateTagHelper<global::DivTagHelper>();
            BeginContext(593, 130, false);
            EndContext();
            BeginContext(723, 6, true);
            EndContext();
            __DivTagHelper = CreateTagHelper<global::DivTagHelper>();
            BeginContext(498, 237, false);
            EndContext();
            BeginContext(735, 6, true);
            EndContext();
            BeginContext(765, 10, true);
            EndContext();
            BeginContext(814, 83, true);
            EndContext();
            __DivTagHelper = CreateTagHelper<global::DivTagHelper>();
            BeginContext(775, 128, false);
            EndContext();
            BeginContext(903, 6, true);
            EndContext();
            __DivTagHelper = CreateTagHelper<global::DivTagHelper>();
            BeginContext(741, 174, false);
            EndContext();
            BeginContext(915, 11, true);
            EndContext();
        }
        #pragma warning restore 1998
#line 6 "TestFiles/IntegrationTests/CodeGenerationTests/RazorPagesWithoutModel.cshtml"
            
    public IActionResult OnPost(Customer customer)
    {
        Name = customer.Name;
        return Redirect("~/customers/inlinepagemodels/");
    }

    public string Name { get; set; }

    public class Customer
    {
        public string Name { get; set; }
    }

#line default
#line hidden
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<TestFiles_IntegrationTests_CodeGenerationTests_RazorPagesWithoutModel_cshtml> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<TestFiles_IntegrationTests_CodeGenerationTests_RazorPagesWithoutModel_cshtml>)PageContext?.ViewData;
        public TestFiles_IntegrationTests_CodeGenerationTests_RazorPagesWithoutModel_cshtml Model => ViewData.Model;
    }
}
