Shader "CGP/Retroreflector" //TO DO: make implement textures for retroreflection and diffuse
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
            Pass 
            {
              Tags { "LightMode" = "ForwardBase" }
              // pass for ambient light and first light source

               CGPROGRAM

               #pragma vertex vert  
               #pragma fragment frag 

               #include "UnityCG.cginc"
               uniform float4 _LightColor0; // color of light source (from "Lighting.cginc")

                 // User-specified properties
                 uniform float4 _Color;
                 uniform float4 _SpecColor;
                 uniform float _Shininess;
                 uniform sampler2D _RetroReflectionTex;
                 uniform sampler2D _DiffuseTexture;

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

                 vertexOutput vert(vertexInput input)
                 {
                    vertexOutput output;

                    float4x4 modelMatrix = unity_ObjectToWorld;
                    float4x4 modelMatrixInverse = unity_WorldToObject;

                    output.tex = input.texcoord;
                    output.posWorld = mul(modelMatrix, input.vertex);
                    output.normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
                    output.pos = UnityObjectToClipPos(input.vertex);
                    return output;
                 }

                 float4 frag(vertexOutput input) : COLOR
                 {
                    float3 normalDirection = normalize(input.normalDir);

                    float3 viewDirection = normalize(
                       _WorldSpaceCameraPos - input.posWorld.xyz);
                    float3 lightDirection;
                    float attenuation;

                    if (0.0 == _WorldSpaceLightPos0.w) // directional light?
                    {
                       attenuation = 1.0; // no attenuation
                       lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                    }
                    else // point or spot light
                    {
                       float3 vertexToLightSource =
                          _WorldSpaceLightPos0.xyz - input.posWorld.xyz;
                       float distance = length(vertexToLightSource);
                       attenuation = 1.0 / (distance * distance); //square attenuation 
                       lightDirection = normalize(vertexToLightSource);
                    }

                    float3 ambientLighting =
                        UNITY_LIGHTMODEL_AMBIENT.rgb * _Color.rgb;

                    float3 diffuseReflection =
                       attenuation * _LightColor0.rgb * _Color.rgb
                       * max(0.0, dot(normalDirection, lightDirection));

                    float3 specularReflection;
                    if (dot(normalDirection, lightDirection) < 0.0) // light source on the wrong side?
                    {
                        specularReflection = float3(0.0, 0.0, 0.0); // no specular reflection
                    }
                    else // light source on the right side
                    {
                        specularReflection = attenuation * _LightColor0.rgb * _SpecColor.rgb * pow(max(0.0, dot(reflect(-lightDirection, normalDirection), viewDirection)), _Shininess);
                    }
                    

                  return float4(ambientLighting + diffuseReflection * tex2D(_DiffuseTexture, input.tex.xy) + specularReflection, 1.0);
                }
            ENDCG
        }
        Pass 
        {
            Tags { "LightMode" = "ForwardAdd" }// pass for additional light sources
            Blend One One // additive blending 

            CGPROGRAM

            #pragma vertex vert  
            #pragma fragment frag 

            #include "UnityCG.cginc"
            //#include "Lighting.cginc"
            #include "AutoLight.cginc" 

            uniform float4 _LightColor0; // color of light source (from "Lighting.cginc")
            uniform float4x4 unity_WorldToLight; // transformation from world to light space (from Autolight.cginc)
            uniform sampler2D _LightTexture0; // cookie alpha texture map (from Autolight.cginc)

            // User-specified properties
            uniform float4 _Color;
            uniform float4 _SpecColor;
            uniform float _Shininess;
            uniform float _RetroPower;
            uniform sampler2D _RetroReflectionTex;  //Texture for retro reflectiveness Strength
            uniform sampler2D _DiffuseTexture;

            struct vertexInput {
               float4 vertex : POSITION;
               float3 normal : NORMAL;
               float4 texcoord : TEXCOORD0;
            };
            struct vertexOutput {
               
               float4 pos : SV_POSITION;
               float4 posWorld : TEXCOORD0;     // position of the vertex (and fragment) in world space 
               float4 posLight : TEXCOORD1;     // position of the vertex (and fragment) in light space
               float3 normalDir : TEXCOORD2;    // surface normal vector in world space
               float4 tex: TEXCOORD3;      // texture for diffuse and retro reflection
            };

            vertexOutput vert(vertexInput input)
            {
               vertexOutput output;

               float4x4 modelMatrix = unity_ObjectToWorld;
               float4x4 modelMatrixInverse = unity_WorldToObject;

               output.tex = input.texcoord;
               output.posWorld = mul(modelMatrix, input.vertex);
               output.posLight = mul(unity_WorldToLight, output.posWorld);
               output.normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
               output.pos = UnityObjectToClipPos(input.vertex);
               return output;
            }

            float4 frag(vertexOutput input) : SV_Target
            {
                float3 normalDirection = normalize(input.normalDir);

                float3 viewDirection = normalize(_WorldSpaceCameraPos - input.posWorld.xyz);
                float3 lightDirection;
                float attenuation;

                if (0.0 == _WorldSpaceLightPos0.w) // directional light?
                {
                    attenuation = 1.0; // no attenuation
                    lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                }
                else // point or spot light
                {
                    float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - input.posWorld.xyz;
                    float distance = length(vertexToLightSource);
                    attenuation = 1.0 / (distance * distance); //square attenuation 
                    lightDirection = normalize(vertexToLightSource);
                }

                float cookieAttenuation = 1.0;
                if (1.0 != unity_WorldToLight[3][3])  // spotlight (i.e. not a point light)?
                {
                    cookieAttenuation = tex2D(_LightTexture0, input.posLight.xy / input.posLight.w + float2(0.5, 0.5)).a;
                }

                float3 diffuseReflection = cookieAttenuation * attenuation * _LightColor0.rgb * _Color.rgb * max(0.0, dot(normalDirection, lightDirection));

                float3 specularReflection;
                if (dot(normalDirection, lightDirection) < 0.0)// light source on the wrong side?
                {
                    specularReflection = float3(0.0, 0.0, 0.0); // no specular reflection
                }
                else // light source on the right side
                {
                    specularReflection = cookieAttenuation * attenuation * _LightColor0.rgb * _SpecColor.rgb * pow(max(0.0, dot(reflect(-lightDirection, normalDirection), viewDirection)), _Shininess);
                }
                
                float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - input.posWorld.xyz;    
                float distance = length(vertexToLightSource) / _RetroPower;
                attenuation = 1.0 / distance; // linear attenuation 
                
                float3 retroReflection;
                if (dot(normalDirection, lightDirection) < 0.707) // light source on the wrong side or more than 45 degrees  off angle?
                {
                    retroReflection = float3(0.0, 0.0, 0.0); // no retro reflection
                }
                else // light source on the right side
                {
                    retroReflection = cookieAttenuation * attenuation * _LightColor0.rgb * pow(max(0.0, dot(lightDirection, viewDirection)), 50);
                }

                 return float4(diffuseReflection * tex2D(_DiffuseTexture, input.tex.xy) + specularReflection + retroReflection * tex2D(_RetroReflectionTex, input.tex.xy) * tex2D(_DiffuseTexture, input.tex.xy), 1.0); // no ambient lighting in this pass
                }
            ENDCG
        }
    }
    //Fallback "Specular"
}