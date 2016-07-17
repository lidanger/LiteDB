﻿using System;

namespace LiteDB.Plataform
{
    public interface IEncryption : IDisposable
    {
        byte[] Encrypt(byte[] bytes);
        byte[] Decrypt(byte[] encryptedValue);
    }
}