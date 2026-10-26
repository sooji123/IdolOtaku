using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace UnityToolbarExtender.Examples
{
	static class ToolbarStyles
	{
		public static readonly GUIStyle commandButtonStyle;

		static ToolbarStyles()
		{
			commandButtonStyle = new GUIStyle("Command")
			{
				fontSize = 10,
				alignment = TextAnchor.MiddleCenter,
				imagePosition = ImagePosition.ImageAbove,
				fontStyle = FontStyle.Bold
			};
		}
	}

	[InitializeOnLoad]
	public class SceneSwitchRightButton
	{
		static SceneSwitchRightButton()
		{
			ToolbarExtender.LeftToolbarGUI.Add(OnLeftToolbarGUI);
			ToolbarExtender.RightToolbarGUI.Add(OnRightToolbarGUI);
		}

		static void OnLeftToolbarGUI()
		{
			GUILayout.FlexibleSpace();

			if(GUILayout.Button(new GUIContent("Start", "Start Title Scene"), ToolbarStyles.commandButtonStyle))
			{
				SceneHelper.StartScene("SplashScene");
			}
		}

		static void OnRightToolbarGUI()
		{
			if(GUILayout.Button(new GUIContent("Splash", "SplashScene"), ToolbarStyles.commandButtonStyle))
			{
				EditorSceneManager.OpenScene("Assets/Scenes/SplashScene.unity");
			}
			
			if(GUILayout.Button(new GUIContent("Selection", "SelectionScene"), ToolbarStyles.commandButtonStyle))
			{
				EditorSceneManager.OpenScene("Assets/Scenes/SelectionScene.unity");
			}

			if(GUILayout.Button(new GUIContent("Game", "GameScene"), ToolbarStyles.commandButtonStyle))
			{
				EditorSceneManager.OpenScene("Assets/Scenes/GameScene.unity");
			}
		}
	}

	static class SceneHelper
	{
		static string sceneToOpen;

		public static void StartScene(string sceneName)
		{
			if(EditorApplication.isPlaying)
			{
				EditorApplication.isPlaying = false;
			}

			sceneToOpen = sceneName;
			EditorApplication.update += OnUpdate;
		}

		static void OnUpdate()
		{
			if (sceneToOpen == null ||
			    EditorApplication.isPlaying || EditorApplication.isPaused ||
			    EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode)
			{
				return;
			}

			EditorApplication.update -= OnUpdate;

			if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
			{
				string[] guids = AssetDatabase.FindAssets("t:scene " + sceneToOpen, null);
				if (guids.Length == 0)
				{
					Debug.LogWarning("Couldn't find scene file");
				}
				else
				{
					string scenePath = AssetDatabase.GUIDToAssetPath(guids[0]);
					EditorSceneManager.OpenScene(scenePath);
					EditorApplication.isPlaying = true;
				}
			}
			sceneToOpen = null;
		}
	}
}