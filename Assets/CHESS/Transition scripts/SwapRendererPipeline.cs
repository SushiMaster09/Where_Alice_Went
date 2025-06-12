using UnityEngine;
using UnityEngine.Rendering;

public class SwapRenderPipeline : MonoBehaviour {
    public RenderPipelineAsset pipeline2D;
    public RenderPipelineAsset ultraURP;
    public static RenderPipelineAsset pipeline2Dstat;
    public static RenderPipelineAsset ultraURPstat;
    private void Start() {
        if (pipeline2Dstat = null) {
            pipeline2Dstat = pipeline2D;
            ultraURPstat = ultraURP;
        }
    }
    public static void UltraAnd2D() {
        if (GraphicsSettings.defaultRenderPipeline == pipeline2Dstat) {
            GraphicsSettings.defaultRenderPipeline = ultraURPstat;
        }
        else {
            GraphicsSettings.defaultRenderPipeline = pipeline2Dstat;
        }
    }
}