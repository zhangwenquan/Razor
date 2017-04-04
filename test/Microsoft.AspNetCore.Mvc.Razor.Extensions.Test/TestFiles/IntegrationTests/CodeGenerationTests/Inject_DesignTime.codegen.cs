namespace AspNetCore
{
    #line hidden
    using System;
    using System.Threading.Tasks;
#line 1 "TestFiles/IntegrationTests/CodeGenerationTests/Inject.cshtml"
using MyNamespace;

#line default
#line hidden
    public class TestFiles_IntegrationTests_CodeGenerationTests_Inject_cshtml : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(18, 2, true);
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public MyApp MyPropertyName { get; private set; }
    }
}
