import { PerspectiveCamera } from "@react-three/drei";
import { useFrame } from "@react-three/fiber";
import { Suspense, useRef, useState } from "react";

export default function MainCamera(props) {
  const camRef = useRef();
  /*
  ? Failed attempt at making the camera shake
  const [yaw, redefineYaw] = useState(0);
  const [pitch, redefinePitch] = useState(0);
  const [roll, redefineRoll] = useState(0);

  var yawScale = 0;
  var pitchScale = 0;
  var rollScale = 0;

  useFrame(() => redefinePitch(Math.sin((pitchScale += 0.0006))));
  useFrame(() => redefineRoll(Math.sin((rollScale += 0.0006))));
  useFrame(() => redefineYaw(Math.sin((yawScale += 0.0006))));
  useFrame(() => {
    camRef.current.rotation.set(pitch, yaw, roll);
  });
  console.log(yaw, pitch, roll);
  console.log(pitchScale, yawScale, rollScale);
  */

  return (
    <PerspectiveCamera
      makeDefault
      position={[props.pos[0], props.pos[1], props.pos[2]]}
      fov={70}
      rotation={[props.rot[0], props.rot[1], props.rot[2]]}
      ref={camRef}
    />
  );
}
