// using System;
// using UnityEngine;
// using UnityEngine.EventSystems;
// using UnityEngine.SceneManagement;
//
// using Parameters;
// using UniRx;
// using Object = UnityEngine.Object;
//
// namespace Utilities
// {
// 	public enum ActionType
// 	{
// 		YesOption,
// 		NoOption,
// 		QuitOption,
// 		InfoOption,
// 		BuyOption,
// 		MessageOption,
// 		TransitionOption
// 	}
// 	public static class PopupHelpers
// 	{
// 		public static PopupParameter PassParamPopup()
// 		{
// 			GameObject go = GameObject.FindGameObjectWithTag(Constants.ParamsTag);
// 			if (GameObject.FindGameObjectWithTag(Constants.ParamsTag) == null)
// 			{
// 				GameObject paramObject = new GameObject(nameof(PopupParameter));
// 				paramObject.tag = Constants.ParamsTag;
// 				PopupParameter popUpParameter = paramObject.AddComponent<PopupParameter>();
// 				return popUpParameter;
// 			}
// 			return go.GetComponent<PopupParameter>();
// 		}
// 		public static void Show(string name)
// 		{
// 			int index = SceneManager.sceneCount;
// 			var scene = SceneManager.GetActiveScene();
// 			// SetEventSystem(scene, false);
// 			SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive).completed += delegate (AsyncOperation op)
// 			{
// 				SetSceneActive(SceneManager.GetSceneAt(index));
// 			};
// 		}
// 		public static void LoadSceneWithLoaidng(string name, Scene current)
// 		{
// 			Show(Constants.LoadingScene);
// 			Observable.Timer(TimeSpan.FromSeconds(1.0f)).Subscribe(_ =>
// 			{
// 				Close(current);
// 				// Show(name);
// 			});
// 			Observable.Timer(TimeSpan.FromSeconds(1.5f)).Subscribe(_ =>
// 			{
// 				// Close(current);
// 				Show(name);
// 			});
// 		}
// 		public static void Close()
// 		{
// 			var scene = SceneManager.GetActiveScene();
// 			// SetEventSystem(scene, false);
// 			SceneManager.UnloadSceneAsync(scene).completed += delegate (AsyncOperation operation)
// 			{
// 				SetSceneActive(SceneManager.GetActiveScene());
// 			};
// 		}
//
// 		public static void Close(string name)
// 		{
// 			var scene = SceneManager.GetSceneByName(name);
// 			// SetEventSystem(scene, false);
// 			SceneManager.UnloadSceneAsync(scene).completed += delegate (AsyncOperation operation)
// 			{
// 				SetSceneActive(SceneManager.GetActiveScene());
// 			};
// 		}
//
// 		/// <summary>
// 		/// New close with special sence
// 		/// </summary>
// 		/// <param name="scene"></param>
// 		public static void Close(Scene scene)
// 		{
// 			if (!scene.IsValid() || !scene.isLoaded)
// 			{
// 				Debug.LogWarning("Cannot close an invalid or unloaded scene: " + scene.name);
// 				return;
// 			}
// 			// SetEventSystem(scene, false);
// 			SceneManager.UnloadSceneAsync(scene).completed += delegate (AsyncOperation operation)
// 			{
// 				SetSceneActive();
// 			};
// 		}
//
// 		private static void SetSceneActive(Scene scene)
// 		{
// 			foreach (var raycaster in Object.FindObjectsOfType<BaseRaycaster>())
// 			{
// 				if(IsAdRaycaster(raycaster)) continue;
// 				raycaster.enabled = raycaster.gameObject.scene == scene;
// 			}
//
// 			SceneManager.SetActiveScene(scene);
// 			//SetEventSystem(scene, true);
// 		}
//
// 		private static bool IsAdRaycaster(BaseRaycaster raycaster)
// 		{
// 			string name = raycaster.gameObject.name;
// 			// Bỏ qua các đối tượng quảng cáo
// 			return name.Contains("BannerBottom") ||
// 			       name.Contains("Interstitial") ||
// 			       name.Contains("Rewarded");
// 		}
// 		/// <summary>
// 		/// auto find top scene to active
// 		/// </summary>
// 		private static void SetSceneActive()
// 		{
// 			int index = SceneManager.sceneCount;
// 			var scene = SceneManager.GetSceneAt(index - 1);
//
// 			foreach (var raycaster in Object.FindObjectsOfType<BaseRaycaster>())
// 			{
// 				if(IsAdRaycaster(raycaster)) continue;
// 				raycaster.enabled = raycaster.gameObject.scene == scene;
// 			}
//
// 			if (scene.isLoaded)
// 			{
// 				SceneManager.SetActiveScene(scene);
// 			}
//
// 		//	SetEventSystem(scene, true);
// 		}
//
// 		private static void SetEventSystem(Scene scene, bool isActive)
// 		{
// 			if (!scene.IsValid())
// 			{
// 				Debug.LogError($"Scene is invalid: {scene.name}");
// 				return;
// 			}
//
// 			if (!scene.isLoaded)
// 			{
// 				Debug.LogError($"Scene is not loaded: {scene.name}");
// 				return;
// 			}
//
// 			var gameObjects = scene.GetRootGameObjects();
// 			for (int i = 0; i < gameObjects.Length; i++)
// 			{
// 				var eventSystem = gameObjects[i].GetComponent<EventSystem>();
// 				if (eventSystem == null) continue;
//
// 				eventSystem.gameObject.SetActive(isActive);
// 			}
// 		}
// 	}
// }
