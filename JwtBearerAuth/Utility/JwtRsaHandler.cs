﻿using Auth.Models.Jwt;
using IdentityModel;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JwtBearerAuth.Utility
{
    /// <summary>
    /// Jwt rsa非对称 帮助类
    /// </summary>
    public class JwtRsaHandler
    {
        /// <summary>
        /// 生成Token
        /// </summary>
        /// <param name="jwtSettings"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        public static string GenerateToken(JwtSettings jwtSettings, IEnumerable<Claim> claims)
        {
            // 载荷信息
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);
            // 授权时间
            DateTime authTime = DateTime.UtcNow;
            // 过期时间
            DateTime expireTime = authTime.AddMinutes(jwtSettings.Expire);
            // 通过密钥生成签名证书（对称可逆）

            RsaSecurityKey key = new RsaSecurityKey(RsaHelper.GetRsaKey(false));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSsaPssSha256);

            // 构造生成Token的描述信息
            var tokenDescripor = new SecurityTokenDescriptor()
            {
                Subject = claimsIdentity,
                Expires = expireTime,
                //NotBefore = authTime,
                SigningCredentials = signingCredentials
            };
            // 开始生成Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescripor);
            // 返回Token字符串
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// 获取Token认证配置
        /// </summary>
        /// <returns></returns>
        public static TokenValidationParameters GetTokenValidParamConfig(JwtSettings jwtSettings)
        {
            RSAParameters rSAParameters = RsaHelper.GetRsaKey(true);
            return new TokenValidationParameters
            {
                NameClaimType = JwtClaimTypes.Name,
                RoleClaimType = JwtClaimTypes.Role,
                // ValidIssuer，ValidAudience默认是开启验证的，这时候必须在Claim中加上这两项内容，
                // 而且这里的ValidIssuer，ValidAudience必须Claim中Issuer、Audience值相同
                ValidIssuer = jwtSettings.Issuer, // Token的颁发机构
                ValidAudience = jwtSettings.Audience, // Token颁发给谁
                // 缓冲过期时间，总的Token的有效时长等于这个时间加上jwt的过期时间，如果不配置，默认是5分钟
                ClockSkew = TimeSpan.FromSeconds(0),
                // 签名key
                IssuerSigningKey = new RsaSecurityKey(rSAParameters) // public key

                #region 默认值
                // RequireSignedTokens = true,
                // SaveSigninToken = false,
                // ValidateActor = false,
                // 将下面两个参数设置为false，可以不验证Issuer和Audience，但是不建议这样做。
                // ValidateAudience = true,
                // ValidateIssuer = true, 
                // ValidateIssuerSigningKey = false,
                // 是否要求Token的Claims中必须包含Expires
                // RequireExpirationTime = true,
                // 允许的服务器时间偏移量
                // ClockSkew = TimeSpan.FromSeconds(300),
                // 是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                // ValidateLifetime = true
                #endregion
            };
        }
    }
}
