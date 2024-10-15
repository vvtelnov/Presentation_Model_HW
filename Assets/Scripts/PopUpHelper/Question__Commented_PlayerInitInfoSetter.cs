// using Lessons.Architecture.PM.Player;
// using Lessons.Architecture.PM.PopUpHelper;
// using UnityEngine;
// using Zenject;
//
// namespace Homeworks.PresentationModel.Scripts.PopUpHelper
// {
//     // TODO: Вопрос!
//     // TODO: Я долго думал Оставить эту логику в отдельном файле или перенести ее в CharacterInitInfoSetter. 
//     // TODO: Если ее здесь оставить, то вроде как НЕ нарушается SRP, В модель можно устанавливать начальные данные из других источников, 
//        TODO:     но с другой стороны код усложняется и нарушается Information Expert
//       TODO: Я решил, что в скопе данного дз лучше сделать как я сделал по KISS, а в боевом проекте лучше разделять подобного рода логику.     
//     
//     // public class PlayerInitInfoSetter : IInitializable
//     // {
//     //     private PlayerLevel _playerLevel;
//     //     private UserInfo _userInfo;
//     //     private CharacterStats _characterStats;
//     //     private CharacterInitInfoSetter _characterInitInfoSetter;
//     //
//     //     [Inject]
//     //     public void Construct(PlayerLevel playerLevel, UserInfo userInfo, CharacterStats characterStats, CharacterInitInfoSetter characterInitInfoSetter)
//     //     {
//     //         _playerLevel = playerLevel;
//     //         _userInfo = userInfo;
//     //         _characterStats = characterStats;
//     //         _characterInitInfoSetter = characterInitInfoSetter;
//     //     }
//     //
//     //     public void Initialize()
//     //     {
//     //         SetInitInfo();
//     //     }
//     //
//     //     private void SetInitInfo()
//     //     {
//     //         Debug.Log("Init Info setting");
//     //         _playerLevel.CurrentLevel = _characterInitInfoSetter.Lvl;
//     //         _playerLevel.CurrentExperience = _characterInitInfoSetter.Xp;
//     //         
//     //         _userInfo.ChangeName(_characterInitInfoSetter.Name);
//     //         _userInfo.ChangeDescription(_characterInitInfoSetter.Description);
//     //         _userInfo.ChangeIcon(_characterInitInfoSetter.ProfilePicture);
//     //
//     //         foreach (var key in _characterInitInfoSetter.InitStats.Keys)
//     //         {
//     //             CharacterStat charStat = new(key, _characterInitInfoSetter.InitStats[key]);
//     //             _characterStats.AddStat(charStat);
//     //         }
//     //     }
//     // }
// }