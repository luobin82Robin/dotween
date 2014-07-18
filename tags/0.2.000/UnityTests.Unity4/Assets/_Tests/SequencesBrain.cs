﻿using DG.Tweening;
using System.Text;
using UnityEngine;

public class SequencesBrain : BrainBase
{
	public int loops = 10001;
	public LoopType loopType;
	public GameObject prefab;

	Sequence mainSequence;
	int stepCompleteMS, stepCompleteS1, stepCompleteS2, stepCompleteT1, stepCompleteT2, stepCompleteT3;
	int completeMS, completeS1, completeS2, completeT1, completeT2, completeT3;
	StringBuilder sb = new StringBuilder();

	void Start()
	{
		mainSequence = CreateSequence();
	}

	void Update()
	{
		Debug.Log("UPDATE " + Time.deltaTime);
	}

	void OnGUI()
	{
		DGUtils.OpenGUI();

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Restart")) {
			ResetStepsCounters();
			mainSequence.Restart();
		}
		if (GUILayout.Button("Rewind")) {
			ResetStepsCounters();
			mainSequence.Rewind();
		}
		if (GUILayout.Button("Complete")) mainSequence.Complete();
		if (GUILayout.Button("Flip")) mainSequence.Flip();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("TogglePause")) mainSequence.TogglePause();
		if (GUILayout.Button("PlayForward")) mainSequence.PlayForward();
		if (GUILayout.Button("PlayBackwards")) mainSequence.PlayBackwards();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Kill All")) DOTween.Kill();
		if (GUILayout.Button("Create Sequence")) mainSequence = CreateSequence();
		if (GUILayout.Button("Create Tween")) CreateTween();
		GUILayout.EndHorizontal();

		GUILayout.Space(10);
		sb.Remove(0, sb.Length);
		sb.Append("IsPlaying: ").Append(mainSequence.IsPlaying());
		sb.Append("\nIsBackwards: ").Append(mainSequence.IsBackwards());
		sb.Append("\nPosition: ").Append(mainSequence.Position());
		sb.Append("\nElapsed: ").Append(mainSequence.Elapsed());
		sb.Append("\nCompletedLoops: ").Append(mainSequence.CompletedLoops());
		GUILayout.Label(sb.ToString());

		GUILayout.Space(10);
		sb.Remove(0, sb.Length);
		sb.Append("MAINSequence Steps/Complete: ").Append(stepCompleteMS).Append("/").Append(completeMS);
		sb.Append("\nSequence OUTER Steps/Complete: ").Append(stepCompleteS1).Append("/").Append(completeS1);
		sb.Append("\nSequence INNER Steps/Complete: ").Append(stepCompleteS2).Append("/").Append(completeS2);
		sb.Append("\nMove Steps/Complete: ").Append(stepCompleteT1).Append("/").Append(completeT1);
		sb.Append("\nRotation Steps/Complete: ").Append(stepCompleteT2).Append("/").Append(completeT2);
		sb.Append("\nColor Steps/Complete: ").Append(stepCompleteT3).Append("/").Append(completeT3);
		GUILayout.Label(sb.ToString());

		DGUtils.CloseGUI();
	}

	Sequence CreateSequence()
	{
		Transform target = ((GameObject)Instantiate(prefab)).transform;
		Material mat = target.gameObject.renderer.material;

		Sequence seq = DOTween.Sequence()
			.Id("Sequence INNER")
			.OnStart(()=> DGUtils.Log("Sequence INNER Start"))
			.OnStepComplete(()=> { stepCompleteS2++; DGUtils.Log("SEQUENCE INNER Step Complete"); })
			.OnComplete(()=> { completeS2++; });

		seq.AppendInterval(0.5f);
		seq.Append(
			target.MoveTo(new Vector3(2, 2, 2), 1f).Loops(1, LoopType.Yoyo)
			.Id("Move")
			.OnStart(()=> DGUtils.Log("Move Start"))
			.OnStepComplete(()=> { stepCompleteT1++; DGUtils.Log("Move Step Complete"); })
			.OnComplete(()=> { completeT1++; })
		);
		seq.Append(
			target.RotateTo(new Vector3(0, 225, 2), 1)
			.Id("Rotate")
			.OnStart(()=> DGUtils.Log("Rotate Start"))
			.OnStepComplete(()=> { stepCompleteT2++; DGUtils.Log("Rotate Step Complete"); })
			.OnComplete(()=> { completeT2++; })
		);
		seq.Insert(
			0.5f, mat.ColorTo(Color.green, 1)
			.Id("Color")
			.OnStart(()=> DGUtils.Log("Color Start"))
			.OnStepComplete(()=> { stepCompleteT3++; DGUtils.Log("Color Step Complete"); })
			.OnComplete(()=> { completeT3++; })
		);
		seq.AppendInterval(0.5f);

		Sequence seqPre = DOTween.Sequence()
			.Id("Sequence OUTER")
			.OnStart(()=> DGUtils.Log("Sequence OUTER Start"))
			.OnStepComplete(()=> { stepCompleteS1++; DGUtils.Log("Sequence OUTER Step Complete"); })
			.OnComplete(()=> { completeS1++; });
		seqPre.Append(seq);
		seqPre.PrependInterval(1);

		Sequence mainSeq = DOTween.Sequence(UpdateType.TimeScaleIndependent).Loops(loops, loopType).AutoKill(false)
			.Id("MAIN SEQUENCE")
			.OnStart(()=> DGUtils.Log("MAINSequence Start"))
			.OnStepComplete(()=> { stepCompleteMS++; DGUtils.Log("MAINSEQUENCE Step Complete"); })
			.OnComplete(()=> { completeMS++; });
		mainSeq.Append(seqPre);
		mainSeq.PrependInterval(1);
		target = ((GameObject)Instantiate(prefab)).transform;
		target.position = new Vector3(-5, 0, 0);
		mainSeq.Append(target.MoveTo(Vector3.zero, 1));

		return mainSeq;
	}

	void CreateTween()
	{
		Transform target = ((GameObject)Instantiate(prefab)).transform;

		target.MoveTo(new Vector3(2, 2, 2), 1f).Loops(1, LoopType.Yoyo)
			.Id("Move (Tween)")
			.Loops(3)
			.OnComplete(()=> Destroy(target.gameObject));
	}

	void ResetStepsCounters()
	{
		stepCompleteMS = stepCompleteS1 = stepCompleteS2 = stepCompleteT1 = stepCompleteT2 = stepCompleteT3 = completeMS = completeS1 = completeS2 = completeT1 = completeT2 = completeT3 = 0;
	}
}