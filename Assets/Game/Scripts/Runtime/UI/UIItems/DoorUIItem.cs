using System;
using GameFramework.Event;
using TMPro;
using UnityEngine;

namespace GameMain
{
    public class DoorUIItem : MonoBehaviour
    {
        private TextMeshProUGUI m_Text;
        private SheepSpawner _spawnerOwner;
        private EndPoint _endOwner;

        private void OnEnable()
        {
            GameEntry.Event.Subscribe(SheepArriveArgs.EventId, OnSheepArrival);
            GameEntry.Event.Subscribe(SheepSpawnArgs.EventId, OnSheepSpawn);
        }

        private void OnDisable()
        {
            GameEntry.Event.Unsubscribe(SheepArriveArgs.EventId, OnSheepArrival);
            GameEntry.Event.Unsubscribe(SheepSpawnArgs.EventId, OnSheepSpawn);
        }

        public void InitSpawnerUI(SheepSpawner owner)
        {
            _spawnerOwner = owner;
            _endOwner = null;
            m_Text = GetComponentInChildren<TextMeshProUGUI>();
            m_Text.text = _spawnerOwner.GetUIString();
            transform.position = Camera.main.WorldToScreenPoint(_spawnerOwner.UITransform.position);
        }

        public void InitEndPointUI(EndPoint owner)
        {
            _endOwner = owner;
            _spawnerOwner = null;
            m_Text = GetComponentInChildren<TextMeshProUGUI>();
            m_Text.text = owner.GetUIString();
            transform.position = Camera.main.WorldToScreenPoint(owner.UITransform.position);
        }


        private void OnSheepArrival(object sender, GameEventArgs args)
        {
            if (!_endOwner) return;
            m_Text.text = _endOwner.GetUIString();
        }

        private void OnSheepSpawn(object sender, GameEventArgs args)
        {
            if (!_spawnerOwner) return;
            m_Text.text = _spawnerOwner.GetUIString();
        }
    }
}