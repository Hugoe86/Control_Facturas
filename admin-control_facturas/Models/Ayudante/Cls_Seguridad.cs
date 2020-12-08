using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace admin_cambios_procesos.Models.Ayudante
{
    public class Cls_Seguridad
    {
        private static string key = "ABCDEFGHIJKLMÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyz0123456789_-";
        //constructorpublic 
        Cls_Seguridad()
        {
            /* Establecer una clave. La misma clave    
             * debe ser utilizada para descifrar    
             * los datos que son cifrados con esta clave.    
             * pueden ser los caracteres que uno desee*/
            //key = "ABCDEFGHIJKLMÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyz0123456789_-";
        }


        /// ************************************************************************************
        /// Nombre          : Encriptar
        /// Descripción     : Metodo que encriptara texto.
        /// Parámetros      : Texto, texto que encriptara
        /// Usuario Creo    :
        /// Fecha Creó      : 
        /// Usuario Modifico:
        /// Fecha Modifico  :
        /// ***********************************************************************************
        public static string Encriptar(String Texto)
        {
            TripleDESCryptoServiceProvider Encriptador = new TripleDESCryptoServiceProvider();
            ICryptoTransform Criptografia;
            MD5CryptoServiceProvider Hash_Md5 = new MD5CryptoServiceProvider();//   se utilizan las clases de encriptación provistas por el Framework  Algoritmo MD5
            byte[] Arreglo_Llave; //    arreglo de bytes donde guardaremos la llave 
            byte[] Arreglo_A_Cifrar;//  arreglo de bytes donde guardaremos el texto que vamos a encriptar
            byte[] Arreglo_Resultante;//  arreglo de bytes donde se guarda la cadena cifrada 

            try
            {
                //   
                Arreglo_Llave = Hash_Md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                Hash_Md5.Clear();

                //  se guarda la llave para que se le realice hashing 
                //  Algoritmo 3DAS 
                Encriptador.Key = Arreglo_Llave;
                Encriptador.Mode = CipherMode.ECB;
                Encriptador.Padding = PaddingMode.PKCS7;

                //  se empieza con la transformación de la cadena 
                Criptografia = Encriptador.CreateEncryptor();
                Arreglo_A_Cifrar = UTF8Encoding.UTF8.GetBytes(Texto);

                Arreglo_Resultante = Criptografia.TransformFinalBlock(Arreglo_A_Cifrar, 0, Arreglo_A_Cifrar.Length);
                Encriptador.Clear();

                //  se regresa el resultado en forma de una cadena 
                return Convert.ToBase64String(Arreglo_Resultante, 0, Arreglo_Resultante.Length);
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }


        }

        /// ************************************************************************************
        /// Nombre          : Desencriptar
        /// Descripción     : Metodo que encriptara texto.
        /// Parámetros      : Texto_Encriptado, texto que se desencriptara
        /// Usuario Creo    :
        /// Fecha Creó      : 
        /// Usuario Modifico:
        /// Fecha Modifico  :
        /// ***********************************************************************************
        public static string Desencriptar(string Texto_Encriptado)
        {
            TripleDESCryptoServiceProvider Desencriptador = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider Hash_Md5 = new MD5CryptoServiceProvider(); //se llama a las clases que tienen los algoritmos de encriptación se le aplica hashing 
            ICryptoTransform Criptografia;
            String Texto = "";
            byte[] Arreglo_Llave;//  convierte el texto en una secuencia de bytes
            byte[] Arragle_A_Descifrar;
            byte[] Arreglo_Resultante;

            try
            {
                Arragle_A_Descifrar = Convert.FromBase64String(Texto_Encriptado);

                //algoritmo MD5 
                Arreglo_Llave = Hash_Md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                Hash_Md5.Clear();

                Desencriptador.Key = Arreglo_Llave;
                Desencriptador.Mode = CipherMode.ECB;
                Desencriptador.Padding = PaddingMode.PKCS7;

                Criptografia = Desencriptador.CreateDecryptor();
                Arreglo_Resultante = Criptografia.TransformFinalBlock(Arragle_A_Descifrar, 0, Arragle_A_Descifrar.Length);
                Desencriptador.Clear();

                //se regresa en forma de cadena
                Texto = UTF8Encoding.UTF8.GetString(Arreglo_Resultante);
            }
            catch (Exception Ex)
            {
                Texto = Texto_Encriptado;
                throw new Exception(Ex.ToString());
            }

            return Texto;
        }// fin de la funcion Desencriptar()
    }
}