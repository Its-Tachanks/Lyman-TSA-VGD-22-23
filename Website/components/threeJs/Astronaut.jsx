import { useAnimations, useGLTF } from "@react-three/drei";
import { useFrame } from "@react-three/fiber";
import { Suspense, useRef } from "react";

export default function Astronaut() {
  const astronautModel = useGLTF("/models/Astronaut.glb");
  const astronautRef = useRef();
  var thing = 0;
  useFrame(
    () => (astronautRef.current.rotation.z = Math.sin((thing += 0.006)) / 10)
  );
  astronautModel.scene.scale.set(0.6, 0.6, 0.6);
  astronautModel.scene.position.set(1.2, -1, 0);
  return (
    <Suspense fallback={null}>
      <primitive object={astronautModel.scene} ref={astronautRef} />
    </Suspense>
  );
}
