using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mala.Core;

#if UNITY_EDITOR
using UnityEngine;
using Assets.Scripts;
#endif


#if UNITY_EDITOR
    public class Singleton< T > : MonoBehaviour where T : MonoBehaviour, new()
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if ( null == _instance )
                {
                    _instance = FindObjectOfType( typeof( T ) ) as T;
                    if ( _instance == null )
                    {
                        Debug.Log( "Not Found T Protocol Object" );
                        
                        return null;
                    }

                    DontDestroyOnLoad( _instance );

                    Debug.Log( $"{ nameof( T ) } Init " );

                    return _instance;
                }

                return _instance;
            }
        }
    }

#else
    public class Singleton< T > where T : Singleton< T >, new()
    {
        public static readonly T Instance = new();
    }

    public class LazySingleton< T > where T : LazySingleton< T >, new()
    {
        private static readonly Lazy< T > _instance = new( () => new T() );

        public static T Instance => _instance.Value;
    }

    public interface ISingleton< T > where T : new()
    {
        private static readonly Lazy< T > _instance = new( () => new T() );
        public static T Instance => _instance.Value;
    }
#endif
