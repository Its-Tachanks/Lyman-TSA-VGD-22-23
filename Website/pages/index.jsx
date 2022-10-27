// Imports
import Head from "next/head";
import Link from "next/link";
import Image from "next/image";
import Script from "next/script";

import { useProgress } from "@react-three/drei";

import { useRef, useEffect, Suspense } from "react";

// CSS imports
import styles from "../styles/pages/Home.module.css";

// Component imports
import TwoColumnHero from "../components/templates/TwoColumnHero";
import HeroImg from "../components/templates/heroParts/HeroImg";
import HeroText from "../components/templates/heroParts/HeroText";

// Page
export default function Home() {
  // Progress on loading for 3d models
  const { progress } = useProgress();

  return (
    <>
      {/* Meta tags */}
      <Head>
        <title>test</title>
        <link rel="icon" href="/favicon.ico" />
      </Head>

      {/* ! Main homepage content */}
      <main>
        <TwoColumnHero flipped="false">
          <HeroImg
            image="https://picsum.photos/200/200"
            imageAlt="Image 1"
            loadAnim="swipeFromLeft"
          />
          <HeroText header="Header 1" text="Text 1" loadAnim="fadeIn" />
        </TwoColumnHero>
        <TwoColumnHero flipped="true">
          <HeroImg
            image="https://picsum.photos/200/200"
            imageAlt="Image 1"
            loadAnim="swipeFromRight"
          />
          <HeroText header="Header 1" text="Text 1" loadAnim="fadeIn" />
        </TwoColumnHero>
        <TwoColumnHero flipped="false">
          <HeroImg
            image="https://picsum.photos/200/200"
            imageAlt="Image 1"
            loadAnim="swipeFromLeft"
          />
          <HeroText header="Header 1" text="Text 1" loadAnim="fadeIn" />
        </TwoColumnHero>
        <TwoColumnHero flipped="true">
          <HeroImg
            image="https://picsum.photos/200/200"
            imageAlt="Image 1"
            loadAnim="swipeFromRight"
          />
          <HeroText header="Header 1" text="Text 1" loadAnim="fadeIn" />
        </TwoColumnHero>
        <TwoColumnHero flipped="false">
          <HeroImg
            image="https://picsum.photos/200/200"
            imageAlt="Image 1"
            loadAnim="swipeFromLeft"
          />
          <HeroText header="Header 1" text="Text 1" loadAnim="fadeIn" />
        </TwoColumnHero>
        <TwoColumnHero flipped="true">
          <HeroImg
            image="https://picsum.photos/200/200"
            imageAlt="Image 1"
            loadAnim="swipeFromRight"
          />
          <HeroText header="Header 1" text="Text 1" loadAnim="fadeIn" />
        </TwoColumnHero>
      </main>
    </>
  );
}
