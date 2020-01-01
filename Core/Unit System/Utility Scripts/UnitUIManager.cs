using UnityEngine;
using UnityEngine.UI;
using System;

namespace UnitUI{
    public class UnitUIManager : MonoBehaviour {
        public Transform unit;
        public Transform HP;

        Soldier myUnit;
        Image healthBar;
        int currentHP;
        int totalHealth;

        // Start is called before the first frame update
        void Start() {
            myUnit = unit.GetComponent<Soldier>();
            healthBar = HP.GetComponent<Image>();
            currentHP = myUnit.unitData.CurrentStatus.currentHP;
            totalHealth = myUnit.TotalHitPoints;
            myUnit.UnitAttacked += unitAttacked;
            myUnit.UnitInit += unitInit;
        }

        // Runs When a Unit is Attacked
        private void unitInit(object sender, EventArgs e) {
            currentHP = myUnit.unitData.CurrentStatus.currentHP;
        }

        // Runs When a Unit is Attacked
        private void unitAttacked(object sender, AttackEventArgs e) {
            currentHP = e.Defender.HitPoints;
            healthBar.fillAmount = (float)currentHP/(float)totalHealth;
        }
    }
}
