namespace AspNetCore
{
    #line hidden
    using System;
    using System.Threading.Tasks;
    public class TestFiles_IntegrationTests_CodeGenerationTests_ModelExpressionTagHelper_cshtml : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<DateTime>
    {
        private global::Microsoft.AspNetCore.Mvc.Razor.Extensions.InputTestTagHelper __Microsoft_AspNetCore_Mvc_Razor_Extensions_InputTestTagHelper = null;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(17, 2, true);
            EndContext();
            BeginContext(141, 4, true);
            EndContext();
            __Microsoft_AspNetCore_Mvc_Razor_Extensions_InputTestTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.Extensions.InputTestTagHelper>();
#line 5 "TestFiles/IntegrationTests/CodeGenerationTests/ModelExpressionTagHelper.cshtml"
__Microsoft_AspNetCore_Mvc_Razor_Extensions_InputTestTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.Now);

#line default
#line hidden
            BeginContext(145, 24, false);
            EndContext();
            BeginContext(169, 2, true);
            EndContext();
            __Microsoft_AspNetCore_Mvc_Razor_Extensions_InputTestTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.Extensions.InputTestTagHelper>();
#line 6 "TestFiles/IntegrationTests/CodeGenerationTests/ModelExpressionTagHelper.cshtml"
__Microsoft_AspNetCore_Mvc_Razor_Extensions_InputTestTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => Model);

#line default
#line hidden
            BeginContext(171, 27, false);
            EndContext();
            BeginContext(198, 2, true);
            EndContext();
        }
        #pragma warning restore 1998
    }
}
