using System;
using System.Collections.Generic;
using Pie;

namespace Sandy.Graphics;

public class RasterizerState : IDisposable
{
    private static Dictionary<RasterizerStateDescription, Pie.RasterizerState> _cachedStates;

    public readonly Pie.RasterizerState PieState;

    static RasterizerState()
    {
        _cachedStates = new Dictionary<RasterizerStateDescription, Pie.RasterizerState>();
    }

    public RasterizerState(RasterizerStateDescription description)
    {
        if (_cachedStates.TryGetValue(description, out PieState))
            return;
        
        Renderer.Instance.LogMessage(LogType.Debug, "Creating new rasterizer state.");
        
        GraphicsDevice device = Renderer.Instance.Device;
            
        PieState = device.CreateRasterizerState(description);
        _cachedStates.Add(description, PieState);
    }

    public static RasterizerState CullNone => new RasterizerState(RasterizerStateDescription.CullNone);

    public static RasterizerState CullClockwise => new RasterizerState(RasterizerStateDescription.CullClockwise);

    public static RasterizerState CullCounterClockwise => new RasterizerState(RasterizerStateDescription.CullCounterClockwise);
    
    public void Dispose()
    {
        // TODO: Proper disposal.
        //PieState.Dispose();
        
        GC.SuppressFinalize(this);
    }
}