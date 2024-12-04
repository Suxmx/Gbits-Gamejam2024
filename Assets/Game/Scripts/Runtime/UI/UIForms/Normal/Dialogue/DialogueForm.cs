//------------------------------------------------------------
// 此文件由工具自动生成
// 生成时间：2024/11/6 14:59:46
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Febucci.UI;
using NodeCanvas.DialogueTrees;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    public partial class DialogueForm : UGuiForm, IDialogueActor
    {
        string IDialogueActor.name => "SELF";
        Texture2D IDialogueActor.portrait => null;

        Sprite IDialogueActor.portraitSprite => null;

        Color IDialogueActor.dialogueColor => Color.white;

        Vector3 IDialogueActor.dialoguePosition => Vector3.zero;

        [SerializeField] private GameObject _choiceBtnPrefab;
        private Image _actorImg;

        private DialogueTreeController _controller;
        private SubtitlesRequestInfo _curContentInfo;
        

        //图中需要获取到Actor的组件，因此应该具体到运行时actor的物体而不是asset，所以初始化的时候根据asset的信息addcomp，结束时销毁
        private List<DialogueActor> _actors = new();
        private Dictionary<int, Button> _choiceBtns = new();

        private TypewriterByCharacter _textWriter;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            GetBindComponents(gameObject);
            _actorImg = m_rect_Actor.GetComponent<Image>();
            _controller = GetComponent<DialogueTreeController>();
            _textWriter = m_tmp_content.GetComponent<TypewriterByCharacter>();

            m_tmp_content.text = "";
            m_tmp_title.text = "";
            m_rect_Choices.gameObject.SetActive(false);
            m_rect_Actor.gameObject.SetActive(false);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            RegisterEvents();
            DialogueTree dlData = (DialogueTree)userData;
            dlData = Instantiate(dlData);
            m_rect_Actor.gameObject.SetActive(true);
            foreach (var actorName in dlData.actorParameters.Select(x => x.name))
            {
                var actor = m_rect_Actor.gameObject.AddComponent<MyDialogueActor>();
                var asset = dlData.GetActorReferenceByName(actorName);
                actor._actorName = actorName;
                if (asset is not null)
                {
                    actor._actorName = asset.name;
                    actor.dialogueColor = asset.dialogueColor;
                    actor.dialoguePosition = asset.dialoguePosition;
                    actor.portrait = asset.portrait;
                    actor.portraitSprite = asset.portraitSprite;
                }

                dlData.SetActorReference(actorName, actor); //统一设置为挂立绘的actor
            }

            _controller.StartDialogue(dlData, this, null);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            RemoveEvents();
            foreach (var actor in _actors)
            {
                Destroy(actor);
            }

            _actors.Clear();
            m_tmp_content.text = "";
            m_tmp_title.text = "";
            m_rect_Choices.gameObject.SetActive(false);
            m_rect_Actor.gameObject.SetActive(false);
        }

        private void RegisterEvents()
        {
            m_btn_Next.onClick.AddListener(RequestNext);

            DialogueTree.OnDialogueStarted += OnDialogueStarted;
            DialogueTree.OnDialoguePaused += OnDialoguePaused;
            DialogueTree.OnDialogueFinished += OnDialogueFinished;
            DialogueTree.OnSubtitlesRequest += OnSubtitlesRequest;
            DialogueTree.OnMultipleChoiceRequest += OnMultipleChoiceRequest;
        }

        private void RemoveEvents()
        {
            m_btn_Next.onClick.RemoveListener(RequestNext);

            DialogueTree.OnDialogueStarted -= OnDialogueStarted;
            DialogueTree.OnDialoguePaused -= OnDialoguePaused;
            DialogueTree.OnDialogueFinished -= OnDialogueFinished;
            DialogueTree.OnSubtitlesRequest -= OnSubtitlesRequest;
            DialogueTree.OnMultipleChoiceRequest -= OnMultipleChoiceRequest;
        }

        void OnDialogueStarted(DialogueTree dlg)
        {
        }

        void OnDialoguePaused(DialogueTree dlg)
        {
        }

        void OnDialogueFinished(DialogueTree dlg)
        {
            GameEntry.UI.TryCloseUIForm(this);
        }

        void OnSubtitlesRequest(SubtitlesRequestInfo info)
        {
            var actor = info.actor;
            m_tmp_title.text = actor.name;
            m_tmp_content.text = "";
            _actorImg.sprite = actor.portraitSprite;
            _actorImg.color = actor.dialogueColor;
            _actorImg.enabled = _actorImg.sprite is not null;

            _curContentInfo = info;
            _textWriter.ShowText(info.statement.text);
            _textWriter.StartShowingText();
        }

        void OnMultipleChoiceRequest(MultipleChoiceRequestInfo info)
        {
            m_rect_Choices.gameObject.SetActive(true);
            //清空上次的choice
            _choiceBtns.Clear();

            //创建按钮
            foreach (var o in info.options)
            {
                var index = o.Value;
                var btn = Instantiate(_choiceBtnPrefab).GetComponent<Button>();
                btn.transform.SetParent(m_rect_Choices);
                btn.transform.localScale = Vector3.one;
                btn.GetComponentInChildren<TextMeshProUGUI>().text = o.Key.text;
                btn.onClick.AddListener(() => DoChoose(info.SelectOption, index));

                _choiceBtns.Add(index, btn);
            }

            //设置按钮顺序
            for (int i = 0; i < _choiceBtns.Count; i++)
            {
                _choiceBtns[i].transform.SetSiblingIndex(i);
            }

            //TODO:Available Time
        }

        private void DoChoose(Action<int> chooseAction, int index)
        {
            foreach (var btn in _choiceBtns.Values)
            {
                for (int i = _choiceBtns.Count - 1; i >= 0; i--)
                {
                    Destroy(btn.gameObject);
                }
            }

            m_rect_Choices.gameObject.SetActive(false);
            chooseAction(index);
        }

        private void RequestNext()
        {
            if (_textWriter.isShowingText)
            {
                _textWriter.SkipTypewriter();
                return;
            }
            
            _curContentInfo.Continue();
        }
    }
}