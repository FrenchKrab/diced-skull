using Godot;
using System;
using System.Collections.Generic;

public class DiceComposition : Control
{
    [Signal]
    public delegate void CompositionDone();

    public static DiceComposition Singleton;

    private const float CursorSpeed = 1000f;

    private Control _abilityButtonParent => GetNode<Control>("MarginContainer/VBox/ScrollButtons/HBox");
    private List<DiceAbilityHolder> _abilityHolders;
    private Label _summaryLabel => GetNode<Label>("MarginContainer/VBox/Summary");

    private Control _splashReadyControl => GetNode<Control>("ReadySplash");
    private AudioStreamPlayer _splashInAudio => GetNode<AudioStreamPlayer>("ReadySplash/SplashInAudio");
    private AudioStreamPlayer _splashOutAudio => GetNode<AudioStreamPlayer>("ReadySplash/SplashOutAudio");
    private AudioStreamPlayer _wrongAudio => GetNode<AudioStreamPlayer>("WrongAudio");

    private int _lastSelectedDice = -1;

    public override void _Ready()
    {


        Singleton = this;

        SetupAbilityHolders();
        SetupAbilityButtons();
        Connect("visibility_changed", this, nameof(OnVisibilityChanged));
        CallDeferred(nameof(OnVisibilityChanged));
    }

    private void OnVisibilityChanged()
    {
        if (!Visible)
            return;

        SetupAbilityButtons();

        _abilityButtonParent.GetChild<AbilityButton>(_abilityButtonParent.GetChildren().Count-1).Button.GrabFocus();
        while (_lastSelectedDice >= 0)
        {
            ClearLastAbility();
        }
    }

    public override void _Process(float delta)
    {
        if (!IsVisibleInTree())
            return;

        if (!_splashReadyControl.Visible)
        {
            if (Input.IsActionJustReleased("ui_cancel"))
            {
                ClearLastAbility();
            }
        }
        else
        {
            if (Input.IsActionJustReleased("ui_cancel"))
            {
                SplashCancel();
            }
            if (Input.IsActionJustPressed("ui_accept"))
                SplashOut();
        }
    }

    private void SplashIn()
    {
        _splashReadyControl.Visible = true;
        _splashReadyControl.GrabFocus();
        _splashInAudio.Play();
    }

    private void SplashOut()
    {
        _splashReadyControl.Visible = false;
        _splashOutAudio.Play();
        this.Visible = false;
        GD.Print(GetSelectedAbilities().Count);
        HeroState.Singleton.ChooseAbilities(GetSelectedAbilities());
    }

    private void SplashCancel()
    {
        _splashReadyControl.Visible = false;
        _abilityButtonParent.GetChild<AbilityButton>(0).Button.GrabFocus();
        ClearLastAbility();
    }

    private void ClearLastAbility()
    {
        if (_lastSelectedDice >= 0)
        {
            _abilityHolders[_lastSelectedDice].ClearAbility();
            _lastSelectedDice--;
            UpdateAbilityTexts();
        }
    }

    private void OnAbilitySelected(string id)
    {
        if (_lastSelectedDice < 5)
        {
            _lastSelectedDice++;
            _abilityHolders[_lastSelectedDice].SetAbility(id);
            UpdateAbilityTexts();
            if (_lastSelectedDice == 5)
            {
                var selectedAbilities = GetSelectedAbilities();
                float intensity = PowerEngine.GetIntensity(selectedAbilities);
                if (intensity <= 0)
                {
                    _wrongAudio.Play();
                }
                else
                {
                    SplashIn();
                }
            }
        }
    }

    private void SetupAbilityButtons()
    {
        foreach (Node child in _abilityButtonParent.GetChildren())
        {
            child.QueueFree();
        }

        const string abilityButtonTemplatePath = "res://dicedhead/ui/nodes/DiceComposition/AbilityButton.tscn";
        var packedAbBtn = ResourceLoader.Load<PackedScene>(abilityButtonTemplatePath);

        foreach (string id in HeroState.Singleton.GetUnlockedAbilities())
        {
            AbilityButton newBtn = (AbilityButton)packedAbBtn.Instance();
            newBtn.SetAbility(id);
            newBtn.Connect(nameof(AbilityButton.AbilityPressed), this, nameof(OnAbilitySelected));
            _abilityButtonParent.AddChild(newBtn);
        }
    }

    private void UpdateAbilityTexts()
    {
        var selectedAbilities = GetSelectedAbilities();
        float intensity = PowerEngine.GetIntensity(selectedAbilities);
        Dictionary<string, int> spread = PowerEngine.GetSpread(selectedAbilities);

        foreach (var holder in _abilityHolders)
            if (holder.AbilityId != null)
                holder.UpdateAbilityText(spread[holder.AbilityId], intensity);

        _summaryLabel.Text = $"Intensity: {intensity}";
    }

    private void SetupAbilityHolders()
    {
        _abilityHolders = new List<DiceAbilityHolder>();
        foreach(var child in GetNode("MarginContainer/VBox/HBox").GetChildren())
        {
            if (child is DiceAbilityHolder childAbHold)
            {
                _abilityHolders.Add(childAbHold);
            }
        }

        for (int i = 0; i < _abilityHolders.Count; ++i)
        {
            _abilityHolders[i].SetDiceNumber(i+1);
        }
    }

    private List<string> GetSelectedAbilities()
    {
        List<string> selectedAbilities = new List<string>();
        foreach (var holder in _abilityHolders)
        {
            GD.Print(holder.AbilityId);
            if (holder.AbilityId != null)
                selectedAbilities.Add(holder.AbilityId);
        }
        return selectedAbilities;
    }
}
