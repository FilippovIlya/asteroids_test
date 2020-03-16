using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityNightPool {
    /// <summary>
    /// Вешается на объект который нужно пулить, отвечает за перемещение объекта в пул и из него
    /// </summary>
    public class PoolObject : MonoBehaviour {
		bool _free=true;
	    bool init = false;
	    bool switchGameObject = true;

        public bool free {
			get {
				return _free;
			}
			private set {
				_free = value;
			}
		}
        public void Init(Pool pool)
	    {
	        if (!init)
	        {
	            init = true;
	            switchGameObject = pool.setup.switchGameObject;
	            if (switchGameObject) gameObject.SetActive(false);
	        }
	    }
        /// <summary>
        /// Достаёт объект из пула
        /// </summary>
        public void Push() {
			free = false;
            if (switchGameObject) gameObject.SetActive(true);
        }
        /// <summary>
        /// Помещает объект в пул
        /// </summary>
        public void Return() {
		    if (init)
		    {
		        free = true;
		        transform.SetParent(PoolManager.parent);
                if (switchGameObject) gameObject.SetActive(false);
		    }
		    else
		    {
		        Destroy(gameObject);
		    }
        }
        private void OnLevelWasLoaded(int level)
        {
            Return();
        }
    }
}