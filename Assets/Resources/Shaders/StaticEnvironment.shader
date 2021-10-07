Shader "ColorGame/StaticEnvironment" //TO DO: make implement textures for retroreflection and diffuse
{

    Properties
    {
       _Color("Diffuse Material Color", Color) = (1,1,1,1)
       _SpecColor("Specular Material Color", Color) = (1,1,1,1)
       _Shininess("Shininess", Float) = 10
       _RetroPower("Retro reflection distance divider", Float) = 5
       _LightTexture0("LightCookie", 2D) = "white" {}
       _DiffuseTexture("DiffuseTexture", 2D) = "white" {}
       _RetroReflectionTex("RetroTex", 2D) = "white" {}
    }

    SubShader
    {        
        Tags {"LightMode" = "UniversalForward"  "RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline" }

        Pass
        {            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"            
    
            struct vertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord : TEXCOORD0;
            };
            struct vertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float4 tex : TEXCOORD2;
            };

            // To make the Unity shader SRP Batcher compatible, declare all
            // properties related to a Material in a a single CBUFFER block with 
            // the name UnityPerMaterial.
            CBUFFER_START(UnityPerMaterial)
                // The following line declares the _Color variable, so that you
                // can use it in the fragment shader.
                uniform float4 _Color;           
                uniform float4 _SpecColor;
                uniform float _Shininess;
                uniform sampler2D _RetroReflectionTex;
                uniform sampler2D _DiffuseTexture; 
                uniform float4 _LightColor0; // color of light source (from "Lighting.cginc")

            CBUFFER_END
            
            vertexOutput vert(vertexInput input)
            {
                vertexOutput output;

                float4x4 modelMatrix = unity_ObjectToWorld;
                float4x4 modelMatrixInverse = unity_WorldToObject;

                output.tex = input.texcoord;
                output.posWorld = mul(modelMatrix, input.vertex);
                output.normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
                output.pos = TransformObjectToHClip(input.vertex);
                return output;
            }
            
            float4 frag(vertexOutput input) : COLOR
            {
                
                
                float3 normalDirection = normalize(input.normalDir);

                float3 viewDirection = normalize(_WorldSpaceCameraPos - input.posWorld.xyz);
                float3 lightDirection;
                
                float3 vertexToLightSource = _WorldSpaceCameraPos.xyz - input.posWorld.xyz;
                lightDirection = normalize(vertexToLightSource);

                float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb * _Color.rgb;

                float3 reflection = _LightColor0.rgb * _Color.rgb * pow(max(0.0, dot(lightDirection, viewDirection)), 50);
                
                return float4(ambientLighting + reflection, 1.0);
                
                return _Color;
            }
            ENDHLSL
        }
    }
}