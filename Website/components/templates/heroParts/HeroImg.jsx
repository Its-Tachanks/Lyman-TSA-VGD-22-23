// Next js imports
import Head from "next/head";
import Link from "next/link";
import Image from "next/image";
import Script from "next/script";

// CSS imports
import styles from "../../../styles/templates/heroParts/HeroImg.module.css";

// Component imports

// Page
export default function HeroImg(props) {
  return (
    <div
      className={`col-md-6 position-relative d-flex justify-content-center align-items-center ${props.loadAnim}`}
      style={{ height: "200px" }}
    >
      <Image
        alt={props.imageAlt}
        src={props.image}
        className="img-fluid rounded border border-2 border-danger"
        layout="fill"
        objectFit="contain"
      />
    </div>
  );
}
