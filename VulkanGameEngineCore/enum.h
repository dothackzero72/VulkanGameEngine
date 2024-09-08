#pragma once

enum TextureUsageEnum {
    kUndefined,
    k2DImageTexture,
    k2DDataTexture,
    k2DDepthTexture,
    k3DImageTexture,
    k3DDataTexture,
    k3DDepthTexture,
};

enum TextureTypeEnum {
    kUndefinedTexture,
    kTextureAtlus,
    kRenderedColorTexture,
    kRenderedDepthTexture,
    kReadableTexture,
    kDiffuseTextureMap,
    kSpecularTextureMap,
    kAlbedoTextureMap,
    kMetallicTextureMap,
    kRoughnessTextureMap,
    kAmbientOcclusionTextureMap,
    kNormalTextureMap,
    kDepthTextureMap,
    kAlphaTextureMap,
    kEmissionTextureMap,
    kPaletteRotationMap,
    kCubeMapTexture,
    kCubeMapDepthTexture,
    kEnvironmentTexture,
    kRenderedCubeMap,
    kBakedTexture
};