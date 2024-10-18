using CatchUpPlatform.API.Shared.Infrastructure.Interfaces.ASP.Configuration.Extensions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace CatchUpPlatform.API.Shared.Infrastructure.Interfaces.ASP.Configuration;

/// <summary>
/// Replaces the default route naming convention with a kebab-case one. 
/// </summary>
/// since 1.0.0
public class KebabCaseRouteNamingConvention : IControllerModelConvention
{
    /// <summary>
    /// Replaces the [controller] token in the route template with the kebab-cased controller name. 
    /// </summary>
    /// <param name="selector">
    /// The selector model to replace the route template in.
    /// </param>
    /// <param name="name">
    /// The name of the controller to kebab-case.
    /// </param>
    /// <returns>
    /// The updated attribute route model with the kebab-cased controller name.
    /// </returns>
    private static AttributeRouteModel? ReplaceControllerTemplate(SelectorModel selector, string name)
    {
        return selector.AttributeRouteModel != null
            ? new AttributeRouteModel { Template = selector.AttributeRouteModel.Template?.Replace("[controller]", name.ToKebabCase()) }
            : null;
    }
    
    /// <summary>
    /// Applies the kebab-case route naming convention to the controller selectors and actions. 
    /// </summary>
    /// <param name="controller">
    /// The controller model to apply the kebab-case route naming convention to.
    /// </param>
    public void Apply(ControllerModel controller)
    {
        foreach (var selector in controller.Selectors) 
            selector.AttributeRouteModel = ReplaceControllerTemplate(selector, controller.ControllerName);

        foreach (var selector in controller.Actions.SelectMany(a => a.Selectors)) 
            selector.AttributeRouteModel = ReplaceControllerTemplate(selector, controller.ControllerName);
        
    }
}