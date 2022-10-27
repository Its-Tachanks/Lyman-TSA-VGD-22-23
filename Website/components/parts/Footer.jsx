// Next js imports
import Head from "next/head";
import Link from "next/link";
import Image from "next/image";

// CSS imports
import styles from "../../styles/parts/Footer.module.css";

// Component imports

// Page
export default function Footer(props) {
  return (
    <section id={`${styles.footer}`} className="container text-center">
      {/* Component content */}
      <div className="row border-top mt-4 py-3">
        <div className={`${styles.footerCol} col`}>© SpaceX</div>
        <div className={`${styles.footerCol} col`}>
          <Image
            src="/img/star.png"
            width="40px"
            height="40px"
            alt="none"
          ></Image>
        </div>
        <div className={`${styles.footerCol} col`}>
          <Link href="/attributions">Attributions</Link>
        </div>
      </div>
    </section>
  );
}
