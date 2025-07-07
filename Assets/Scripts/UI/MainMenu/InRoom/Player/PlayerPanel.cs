using Quantum;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NSMB.UI.MainMenu.Submenus.InRoom {
    public class PlayerPanel : InRoomSubmenuPanel {

        //---Properties
        public override GameObject DefaultSelectedObject => playerList.GetPlayerEntryAtIndex(0).button.gameObject;
        public override bool IsInSubmenu => playerList.OpenDropdown;

        //---Serialized Variables
        [SerializeField] private PlayerListHandler playerList;

        //---Private Variables
        private int selectedIndex;

        public void OnEnable() {
            PlayerListHandler.PlayerRemoved += OnPlayerRemoved;
            PlayerListEntry.PlayerEntrySelected += OnPlayerEntrySelected;
        }

        public void OnDisable() {
            PlayerListHandler.PlayerRemoved -= OnPlayerRemoved;
            PlayerListEntry.PlayerEntrySelected -= OnPlayerEntrySelected;
        }

        public override void Initialize() {
            playerList.Initialize();
        }

        public override bool TryGoBack(out bool playSound) {
            PlayerListEntry dropdown = playerList.OpenDropdown;
            if (dropdown) {
                dropdown.HideDropdown(false);
                playSound = false;
                return false;
            }
            return base.TryGoBack(out playSound);
        }

        public override void Select(bool setDefault) {
            base.Select(setDefault);
            selectedIndex = 0;
        }

        public override void Deselect() {
            base.Deselect();
            selectedIndex = -1;
        }

        private void OnPlayerRemoved(int index) {
            if (selectedIndex == index) {
                PlayerListEntry next = playerList.GetPlayerEntryAtIndex(selectedIndex);
                if (next.player == PlayerRef.None) {
                    selectedIndex--;
                    next = playerList.GetPlayerEntryAtIndex(selectedIndex);
                }

                EventSystem.current.SetSelectedGameObject(next.button.gameObject);
            }
        }

        private void OnPlayerEntrySelected(PlayerListEntry entry) {
            menu.SelectPanel(this, false);
            selectedIndex = playerList.GetPlayerEntryIndexOf(entry);
        }
    }
}
