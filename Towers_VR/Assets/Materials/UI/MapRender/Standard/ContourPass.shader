Shader "FullScreen/ContourPass"
{
    Properties{
        _ContoursAmount("Amount of contour lines", Int) = 0
        _LineSize("The size of each line", Int) = 1
        _Precision("The precision of the line", Float) = 0.1
        _BottomColor("Lower Color", Color) = (0,0,0,1)
        _TopColor("Higher Color", Color) = (0,0,0,1)
        _BaseColor("No Color", Color) = (0,0,0,1)
        _LineColour("Line Color", Color) = (0,0,0,1)
    }
    HLSLINCLUDE

    #pragma vertex Vert

    #pragma target 4.5
    #pragma only_renderers d3d11 ps4 xboxone vulkan metal switch

    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/RenderPass/CustomPass/CustomPassCommon.hlsl"

    // The PositionInputs struct allow you to retrieve a lot of useful information for your fullScreenShader:
    // struct PositionInputs
    // {
    //     float3 positionWS;  // World space position (could be camera-relative)
    //     float2 positionNDC; // Normalized screen coordinates within the viewport    : [0, 1) (with the half-pixel offset)
    //     uint2  positionSS;  // Screen space pixel coordinates                       : [0, NumPixels)
    //     uint2  tileCoord;   // Screen tile coordinates                              : [0, NumTiles)
    //     float  deviceDepth; // Depth from the depth buffer                          : [0, 1] (typically reversed)
    //     float  linearDepth; // View space Z coordinate                              : [Near, Far]
    // };

    // To sample custom buffers, you have access to these functions:
    // But be careful, on most platforms you can't sample to the bound color buffer. It means that you
    // can't use the SampleCustomColor when the pass color buffer is set to custom (and same for camera the buffer).
    // float4 SampleCustomColor(float2 uv);
    // float4 LoadCustomColor(uint2 pixelCoords);
    // float LoadCustomDepth(uint2 pixelCoords);
    // float SampleCustomDepth(float2 uv);

    // There are also a lot of utility function you can use inside Common.hlsl and Color.hlsl,
    // you can check them out in the source code of the core SRP package.
    int _ContoursAmount;
    int _LineSize;
    float _Precision;
    float3 _BottomColor;
    float3 _TopColor;
    float3 _BaseColor;
    float3 _LineColour;
    sampler2D _MainTex;
    float4 FullScreenPass(Varyings varyings) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(varyings);
        float depth = LoadCameraDepth(varyings.positionCS.xy);
        PositionInputs posInput = GetPositionInput(varyings.positionCS.xy, _ScreenSize.zw, depth, UNITY_MATRIX_I_VP, UNITY_MATRIX_V);
        float3 viewDirection = GetWorldSpaceNormalizeViewDir(posInput.positionWS);
        float4 color = float4(0.0, 0.0, 0.0, 0.0);

        // Load the camera color buffer at the mip 0 if we're not at the before rendering injection point
        if (_CustomPassInjectionPoint != CUSTOMPASSINJECTIONPOINT_BEFORE_RENDERING)
            color = float4(CustomPassLoadCameraColor(varyings.positionCS.xy, 0), 1);
        // Add your custom pass code here
        float amount = abs(round(_ContoursAmount));
        float CameraDepth = LoadCameraDepth(posInput.positionSS);
        float lineOffset = 1.0/amount;
        
        int ClosestLine;
        float ClosestLinedif = 1;
        color.rgb = lineOffset;
        //Can be optimized
        for(int i = 1; i < amount; i++)
        {
            float linedif = abs(CameraDepth-lineOffset*i);
            if(ClosestLinedif>linedif){
                ClosestLinedif = linedif;
                ClosestLine = i;
            }
        }
        int colorGrade;
        for(int i = 0; (i*lineOffset-_Precision) <= CameraDepth; i++)
        {
            colorGrade = i;
        }
        if(colorGrade == 0){
            color.rgb = _BaseColor;
        }
        else{
            float range = (colorGrade)/amount;
            color.rgb = lerp(_BottomColor,_TopColor,range);
        }
        
        

        bool Colored = false;
        if(abs(lineOffset*ClosestLine - CameraDepth)<_Precision){
            //if CameraDepth belongs to a line zone
            //color.rgb = 0;
            for(int i = -_LineSize;i<_LineSize && !Colored;i++){ // this loop will continue while pixels are left to check or a black pixel has been found
                //every row pixel
                uint2 offsetVector = uint2(i,0);
                for(int j = -_LineSize;j<_LineSize && !Colored;j++){ // this loop will continue while pixels are left to check or a black pixel has been found
                    //every column pixel
                    offsetVector.y = j;
                    float CheckedPixelDepth = LoadCameraDepth(posInput.positionSS + offsetVector);
                    if(CheckedPixelDepth < (lineOffset*ClosestLine-_Precision)){
                        Colored = true;
                        color.rgb = _LineColour;
                    }
                }
                
            }
        }
        //color.rgb = ClosestLinedif;
        // Fade value allow you to increase the strength of the effect while the camera gets closer to the custom pass volume
        return color;
    }

    ENDHLSL

    SubShader
    {
        Pass
        {
            Name "Contour Effect Pass"

            ZWrite Off
            ZTest Always
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off

            HLSLPROGRAM
                #pragma fragment FullScreenPass
            ENDHLSL
        }
    }
    Fallback Off
}
