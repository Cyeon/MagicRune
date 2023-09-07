#ifndef MAGICINCLUDE_INCLUDED
#define MAGICINCLUDE_INCLUDED

void uvRotateSize_float(float4 _uv,float _size,float _rotate, out float4 Out)
{
    _rotate = _rotate*(3.14159265359/180);
    float2 ruv = float2(((_uv.x - 0.5) * 1/_size) + 0.5 ,((_uv.y - 0.5) * 1/_size) + 0.5);
    ruv = ruv - float2(0.5, 0.5);  
    ruv = float2(ruv.x * cos(_rotate) - ruv.y * sin(_rotate),ruv.x * sin(_rotate) + ruv.y * cos(_rotate));  
    ruv += float2(0.5, 0.5);

    Out = float4(ruv.x,ruv.y,_uv.z,_uv.w);
}


void Dissolve_float (float2 _uv,float4 _Dtex,float _DissolveIn ,float _DissolveOut,float4 _Color,float4 _tex, out float4 Out){
               
    float4 spheric = saturate( distance( _uv ,float2(0.5,0.5)) );
    spheric =saturate(spheric + spheric * _Dtex);
               
    float4 spheric01A =saturate( saturate( (spheric + lerp(-1.01,0.01,1-_DissolveIn ))*100));
    float4 spheric01B = saturate( (spheric + lerp(-1.01,0.01,1-_DissolveIn- 0.01))*100);
    spheric01B = spheric01A * (1-spheric01B) * _Color * _Color.a;

    _tex = _tex * spheric01A + spheric01B * _tex.a;

    float4 spheric02A = saturate( ((1-spheric) + lerp(-1.01,0.01,_DissolveOut+ 0.01))*100);
    float4 spheric02B = saturate( ((1-spheric) + lerp(-1.01,0.01,_DissolveOut))*100);
    spheric02B =spheric02A * (1-spheric02B) * _Color * _Color.a;
    
    _tex = _tex * spheric02A + spheric02B * _tex.a;
    Out = _tex;
}


void TextureLight_float (float4 _tex,float _light,float4 _color,float _lightGain, out float4 Out){
    float4 RtexB = smoothstep( 1-_light,1,_tex);
    RtexB = RtexB * 0.2 + RtexB *  _color * _lightGain;
    _tex.rgb = _tex.rgb * _color.rgb + RtexB.rgb;
    _tex = _tex * _color.a *_tex.a ;
    Out = _tex;
}

void DTexSet_float (float4 _uvD , float4 _DissolveTex_ST , out float4 Out){
    Out = float4((_uvD.x- 0.5) * _DissolveTex_ST.x+ _DissolveTex_ST.z,(_uvD.y- 0.5) * _DissolveTex_ST.y+ _DissolveTex_ST.w,_uvD.z,_uvD.w);
}

void TexProjCoord_float (float4 _uv , out float2 Out){
    Out = _uv.xy / _uv.w;
}

#endif //MAGICINCLUDE_INCLUDED