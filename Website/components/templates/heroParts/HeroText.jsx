// Next js imports
import Head from "next/head";
import Link from "next/link";
import Image from "next/image";
import Script from "next/script";

// CSS imports
import styles from "../../../styles/templates/heroParts/HeroText.module.css";

// Component imports

// Page
export default function HeroText(props) {
  return (
    <div className={`col-md-6 text-bg-dark ${props.loadAnim}`}>
      <h1 className="">{props.header}</h1>
      <p className="mb-0">{props.text}</p>
    </div>
  );
}
