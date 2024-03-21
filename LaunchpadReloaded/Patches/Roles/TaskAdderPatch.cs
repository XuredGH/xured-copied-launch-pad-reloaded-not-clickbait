using AmongUs.GameOptions;
using HarmonyLib;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.Text;
using LaunchpadReloaded.API.Roles;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LaunchpadReloaded.Patches.Roles;

[HarmonyPatch]
public static class TaskAdderPatch
{
    public static TaskFolder RolesFolder;

    [HarmonyPostfix, HarmonyPatch(typeof(TaskAdderGame), nameof(TaskAdderGame.Begin))]
    public static void AddRolesFolder(TaskAdderGame __instance)
    {
        RolesFolder = Object.Instantiate(__instance.RootFolderPrefab, __instance.transform);
        RolesFolder.gameObject.SetActive(false);
        RolesFolder.FolderName = "Roles";
        RolesFolder.name = "RolesFolder";
        __instance.Root.SubFolders.Add(RolesFolder);
        __instance.GoToRoot();
    }

    [HarmonyPostfix, HarmonyPatch(typeof(TaskAddButton), "Role", MethodType.Setter)]
    public static void RoleGetterPatch(TaskAddButton __instance)
    {
        if (__instance.role is ICustomRole lpRole)
        {
            if (lpRole.IsOutcast) __instance.FileImage.color = Color.gray;
        }

        __instance.RolloverHandler.OutColor = __instance.FileImage.color;
    }

    // yes it might be crazy patching the entire method, but i tried so many other methods and only this works :cry:
    [HarmonyPrefix, HarmonyPatch(typeof(TaskAdderGame), nameof(TaskAdderGame.ShowFolder))]
    public static bool ShowPatch(TaskAdderGame __instance, [HarmonyArgument(0)] TaskFolder taskFolder)
    {
        StringBuilder stringBuilder = new StringBuilder(64);
        __instance.Hierarchy.Add(taskFolder);
        for (int i = 0; i < __instance.Hierarchy.Count; i++)
        {
            stringBuilder.Append(__instance.Hierarchy[i].FolderName);
            stringBuilder.Append("\\");
        }
        __instance.PathText.text = stringBuilder.ToString();
        for (int j = 0; j < __instance.ActiveItems.Count; j++)
        {
            Object.Destroy(__instance.ActiveItems[j].gameObject);
        }
        __instance.ActiveItems.Clear();
        float num = 0f;
        float num2 = 0f;
        float num3 = 0f;
        for (int k = 0; k < taskFolder.SubFolders.Count; k++)
        {
            TaskFolder taskFolder2 = Object.Instantiate(taskFolder.SubFolders[k], __instance.TaskParent);
            taskFolder2.gameObject.SetActive(true);
            taskFolder2.Parent = __instance;
            taskFolder2.transform.localPosition = new Vector3(num, num2, 0f);
            taskFolder2.transform.localScale = Vector3.one;
            num3 = Mathf.Max(num3, taskFolder2.Text.bounds.size.y + 1.1f);
            num += __instance.folderWidth;
            if (num > __instance.lineWidth)
            {
                num = 0f;
                num2 -= num3;
                num3 = 0f;
            }
            __instance.ActiveItems.Add(taskFolder2.transform);
            if (taskFolder2 != null && taskFolder2.Button != null)
            {
                ControllerManager.Instance.AddSelectableUiElement(taskFolder2.Button, false);
                if (!string.IsNullOrEmpty(__instance.restorePreviousSelectionByFolderName) && taskFolder2.FolderName.Equals(__instance.restorePreviousSelectionByFolderName))
                {
                    __instance.restorePreviousSelectionFound = taskFolder2.Button;
                }
            }
        }
        bool flag = false;
        List<PlayerTask> list = new List<PlayerTask>();
        foreach (var item in taskFolder.Children) list.Add(item);

        for (int l = 0; l < list.Count; l++)
        {
            TaskAddButton taskAddButton = Object.Instantiate(__instance.TaskPrefab);
            taskAddButton.MyTask = list[l];
            if (taskAddButton.MyTask.TaskType == TaskTypes.DivertPower)
            {
                SystemTypes targetSystem = taskAddButton.MyTask.Cast<DivertPowerTask>().TargetSystem;
                taskAddButton.Text.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.DivertPowerTo, new Il2CppSystem.Object[]
                {
                        DestroyableSingleton<TranslationController>.Instance.GetString(targetSystem)
                });
            }
            else if (taskAddButton.MyTask.TaskType == TaskTypes.FixWeatherNode)
            {
                int nodeId = ((WeatherNodeTask)taskAddButton.MyTask).NodeId;
                taskAddButton.Text.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FixWeatherNode, Array.Empty<Il2CppSystem.Object>()) + " " + DestroyableSingleton<TranslationController>.Instance.GetString(WeatherSwitchGame.ControlNames[nodeId],
                    Array.Empty<Il2CppSystem.Object>());
            }
            else
            {
                taskAddButton.Text.text = DestroyableSingleton<TranslationController>.Instance.GetString(taskAddButton.MyTask.TaskType);
            }
            __instance.AddFileAsChild(taskFolder, taskAddButton, ref num, ref num2, ref num3);
            if (taskAddButton != null && taskAddButton.Button != null)
            {
                ControllerManager.Instance.AddSelectableUiElement(taskAddButton.Button, false);
                if (__instance.Hierarchy.Count != 1 && !flag)
                {
                    TaskFolder component = ControllerManager.Instance.CurrentUiState.CurrentSelection.GetComponent<TaskFolder>();
                    if (component != null)
                    {
                        __instance.restorePreviousSelectionByFolderName = component.FolderName;
                    }
                    ControllerManager.Instance.SetDefaultSelection(taskAddButton.Button, null);
                    flag = true;
                }
            }
        }

        if (taskFolder.FolderName == "Roles") // idk why only this works???
        {
            for (int m = 0; m < DestroyableSingleton<RoleManager>.Instance.AllRoles.Length; m++)
            {
                RoleBehaviour roleBehaviour = DestroyableSingleton<RoleManager>.Instance.AllRoles[m];
                if (roleBehaviour.Role != RoleTypes.ImpostorGhost && roleBehaviour.Role != RoleTypes.CrewmateGhost)
                {
                    TaskAddButton taskAddButton2 = Object.Instantiate(__instance.RoleButton);
                    taskAddButton2.SafePositionWorld = __instance.SafePositionWorld;
                    taskAddButton2.Text.text = "Be_" + roleBehaviour.NiceName + ".exe";
                    __instance.AddFileAsChild(RolesFolder, taskAddButton2, ref num, ref num2, ref num3);
                    taskAddButton2.Role = roleBehaviour;
                    if (taskAddButton2 != null && taskAddButton2.Button != null)
                    {
                        ControllerManager.Instance.AddSelectableUiElement(taskAddButton2.Button, false);
                        if (m == 0 && __instance.restorePreviousSelectionFound != null)
                        {
                            ControllerManager.Instance.SetDefaultSelection(__instance.restorePreviousSelectionFound, null);
                            __instance.restorePreviousSelectionByFolderName = string.Empty;
                            __instance.restorePreviousSelectionFound = null;
                        }
                        else if (m == 0)
                        {
                            ControllerManager.Instance.SetDefaultSelection(taskAddButton2.Button, null);
                        }
                    }
                }
            }
        }
        if (__instance.Hierarchy.Count == 1)
        {
            ControllerManager.Instance.SetBackButton(__instance.BackButton);
            return false;
        }
        ControllerManager.Instance.SetBackButton(__instance.FolderBackButton);
        return false;
    }
}