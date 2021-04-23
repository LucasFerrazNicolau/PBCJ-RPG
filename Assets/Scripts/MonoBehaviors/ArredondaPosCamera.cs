using Cinemachine;
using UnityEngine;

public class ArredondaPosCamera : CinemachineExtension
{
    public float PixelsPerUnit = 32; // parâmetro de pixels por unidade utilizado

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage,
        ref CameraState state,
        float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body)
        {
            Vector3 pos = state.FinalPosition;
            Vector3 pos2 = new Vector3(Round(pos.x), Round(pos.y), pos.z);
            state.PositionCorrection += pos2 - pos;
        }
    }

    /// <summary>
    /// Arredonda um valor float
    /// </summary>
    /// <param name="x">Valor float</param>
    /// <returns>Arredondamento do valor</returns>
    float Round(float x)
    {
        return Mathf.Round(x * PixelsPerUnit) / PixelsPerUnit;
    }
}
