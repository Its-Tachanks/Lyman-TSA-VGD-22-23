// Next js imports
import Head from "next/head";
import Link from "next/link";
import Image from "next/image";
import Script from "next/script";

// CSS imports
import styles from "../../styles/templates/TwoColumnHero.module.css";

// Component imports

// Page
export default function TwoColumnHero(props) {
  return (
    <section>
      <div className={`container-sm px-4 py-5 mw-100 ${props.loadAnim} h-100`}>
        <div
          className={`row align-items-center ${
            props.flipped === "true" ? "flex-row-reverse" : ""
          }`}
        >
          {props.children}
        </div>
      </div>
    </section>
  );
}
