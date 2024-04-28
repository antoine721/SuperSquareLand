using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance {  get; private set; }

    [Header("Camera")]
    [SerializeField] private Camera _camera;

    [Header("Profile System")]
    [SerializeField] private CameraProfile _defaultCameraProfile;
    private CameraProfile _currentCameraProfile;
    private float _profileTransitionTimer = 0f;
    private float _profileTransitionDuration = 0f;
    private Vector3 _profileTransitionStartPosition;
    private float _profileTransitionStartSize;

    private Vector3 _profileLastFollowDestination;

    private Vector3 _dampedPosition;



    private void Awake()
    {
        instance = this;
    }
    private void _SetCameraPosition(Vector3 position)
    {
        Vector3 newCameraPosition = _camera.transform.position;
        newCameraPosition.x = position.x;
        newCameraPosition.y = position.y;
        _camera.transform.position = newCameraPosition;
    }

    private void _SetCameraSize(float size)
    {
        _camera.orthographicSize = size;
    }

    private void Start()
    {
        _InitToDefaultProfile();
    }

    private void _SetCameraDampedPosition(Vector3 position)
    {
        _dampedPosition.x = position.x;
        _dampedPosition.y = position.y;
    }

    private void _InitToDefaultProfile()
    {
        _currentCameraProfile = _defaultCameraProfile;
        _SetCameraPosition(_currentCameraProfile.Position);
        _SetCameraSize(_currentCameraProfile.CameraSize);
        _SetCameraDampedPosition(_ClampPositionIntoBounds(_FindCameraNextPosition()));
    }

    private void Update()
    {
        Vector3 nextPosition = _FindCameraNextPosition();
        nextPosition = _ClampPositionIntoBounds(nextPosition);
        nextPosition = _ApplyDamping(nextPosition);
        if (_IsPlayingProfileTransition())
        {
            _profileTransitionTimer += Time.deltaTime;
            Vector3 transitionPosition = _CalculateProfileTransitionPosition(nextPosition);
            _SetCameraPosition(transitionPosition);
            float transitionSize = _CalculateProfileTransitionCameraSize(_currentCameraProfile.CameraSize);
            _SetCameraSize(transitionSize);
        } else
        {
            _SetCameraPosition(nextPosition);
            _SetCameraSize(_currentCameraProfile.CameraSize);
        }
        
    }

    public void EnterProfile(CameraProfile cameraProfile, CameraProfileTransition transition = null)
    {
        _currentCameraProfile = cameraProfile;
        if (transition != null)
        {
            _PlayProfileTransition(transition);
        }
        _SetCameraDampedPosition(_FindCameraNextPosition());
    }

    public void ExitProfile(CameraProfile cameraProfile, CameraProfileTransition transition = null)
    {
        if (_currentCameraProfile != cameraProfile) return;
        _currentCameraProfile = _defaultCameraProfile;
        if (transition != null)
        {
            _PlayProfileTransition(transition);
        }
        _SetCameraDampedPosition(_FindCameraNextPosition());
    }

    private void _PlayProfileTransition(CameraProfileTransition transition)
    {
        _profileTransitionStartPosition = _camera.transform.position;

        _profileTransitionStartSize = _camera.orthographicSize;

        _profileTransitionTimer = 0f;
        _profileTransitionDuration = transition.duration;
    }

    private bool _IsPlayingProfileTransition()

    {
        return _profileTransitionTimer < _profileTransitionDuration;
    }

    private float _CalculateProfileTransitionCameraSize(float endSize)
    {
        float percent = _profileTransitionTimer / _profileTransitionDuration;
        float startSize = _profileTransitionStartSize;
        return Mathf.Lerp(startSize, endSize, percent);
    }

    private Vector3 _CalculateProfileTransitionPosition(Vector3 destination)
    {
        float percent = _profileTransitionTimer / _profileTransitionDuration;
        Vector3 origin = _profileTransitionStartPosition;
        return Vector3.Lerp(origin, destination, percent); 
    }

    private Vector3 _FindCameraNextPosition()
    {
        if(_currentCameraProfile.ProfileType == CameraProfile.CameraProfileType.FollowTarget)
        {
            if(_currentCameraProfile.TargetToFollow != null)
            {
                CameraFollowable targetToFollow = _currentCameraProfile.TargetToFollow;
                _profileLastFollowDestination.x = targetToFollow.FollowPositionX;
                _profileLastFollowDestination.y = targetToFollow.FollowPositionY;
                return _profileLastFollowDestination;
            }
        }
        return _currentCameraProfile.Position;
    }

    private Vector3 _ApplyDamping(Vector3 position)
    {
        if(_currentCameraProfile.UseDampingHorizontally)
        {
            _dampedPosition.x = Mathf.Lerp(_dampedPosition.x, position.x, _currentCameraProfile.HorizontalDampingFactor * Time.deltaTime);
        } else
        {
            _dampedPosition.x = position.x;
        }

        if (_currentCameraProfile.UseDampingVertically)
        {
            _dampedPosition.y = Mathf.Lerp(_dampedPosition.y,position.y, _currentCameraProfile.VerticalDampingFactor * Time.deltaTime);

        } else
        {
            _dampedPosition.y = position.y;
        }

        return _dampedPosition;
    }

    private Vector3 _ClampPositionIntoBounds(Vector3 position)
    {
        if (!_currentCameraProfile.HasBounds)
            return position;

        Rect boundsRect = _currentCameraProfile.BoundsRect;
        Vector3 worldBottomLeft = _camera.ScreenToWorldPoint(new Vector3(0f, 0f));
        Vector3 worldTopRight = _camera.ScreenToWorldPoint(new Vector3(_camera.pixelWidth, _camera.pixelHeight));
        Vector3 worldScreenSize = new Vector3(worldTopRight.x - worldBottomLeft.x, worldTopRight.y - worldBottomLeft.y, 0f);
        Vector3 worldHalfScreenSize = worldScreenSize / 2f;

        if (position.x > boundsRect.xMax - worldHalfScreenSize.x)
            position.x = boundsRect.xMax - worldHalfScreenSize.x;

        if (position.x < boundsRect.xMin + worldHalfScreenSize.x)
            position.x = boundsRect.xMin + worldHalfScreenSize.x;

        if (position.y > boundsRect.yMax - worldHalfScreenSize.y)
            position.y = boundsRect.yMax - worldHalfScreenSize.y;

        if (position.y < boundsRect.yMin + worldHalfScreenSize.y)
            position.y = boundsRect.yMin + worldHalfScreenSize.y;

        return position;
    }




}