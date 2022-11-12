--
-- PostgreSQL database dump
--

-- Dumped from database version 13.8 (Debian 13.8-1.pgdg110+1)
-- Dumped by pg_dump version 14.4

-- Started on 2022-11-06 15:00:28 MSK

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 201 (class 1259 OID 16427)
-- Name: airport; Type: TABLE; Schema: public; Owner: program
--

CREATE TABLE public.airport (
    id integer NOT NULL,
    name character varying(255),
    city character varying(255),
    country character varying(255)
);


ALTER TABLE public.airport OWNER TO program;

--
-- TOC entry 200 (class 1259 OID 16425)
-- Name: airport_id_seq; Type: SEQUENCE; Schema: public; Owner: program
--

CREATE SEQUENCE public.airport_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.airport_id_seq OWNER TO program;

--
-- TOC entry 3007 (class 0 OID 0)
-- Dependencies: 200
-- Name: airport_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: program
--

ALTER SEQUENCE public.airport_id_seq OWNED BY public.airport.id;


--
-- TOC entry 203 (class 1259 OID 16438)
-- Name: flight; Type: TABLE; Schema: public; Owner: program
--

CREATE TABLE public.flight (
    id integer NOT NULL,
    flight_number character varying(20) NOT NULL,
    datetime timestamp with time zone NOT NULL,
    from_airport_id integer,
    to_airport_id integer,
    price integer NOT NULL
);


ALTER TABLE public.flight OWNER TO program;

--
-- TOC entry 202 (class 1259 OID 16436)
-- Name: flight_id_seq; Type: SEQUENCE; Schema: public; Owner: program
--

CREATE SEQUENCE public.flight_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.flight_id_seq OWNER TO program;

--
-- TOC entry 3008 (class 0 OID 0)
-- Dependencies: 202
-- Name: flight_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: program
--

ALTER SEQUENCE public.flight_id_seq OWNED BY public.flight.id;


--
-- TOC entry 2860 (class 2604 OID 16430)
-- Name: airport id; Type: DEFAULT; Schema: public; Owner: program
--

ALTER TABLE ONLY public.airport ALTER COLUMN id SET DEFAULT nextval('public.airport_id_seq'::regclass);


--
-- TOC entry 2861 (class 2604 OID 16441)
-- Name: flight id; Type: DEFAULT; Schema: public; Owner: program
--

ALTER TABLE ONLY public.flight ALTER COLUMN id SET DEFAULT nextval('public.flight_id_seq'::regclass);


--
-- TOC entry 2999 (class 0 OID 16427)
-- Dependencies: 201
-- Data for Name: airport; Type: TABLE DATA; Schema: public; Owner: program
--

COPY public.airport (id, name, city, country) FROM stdin;
1	Пулково	Санкт-Петербург	Россия
2	Шереметьево	Москва	Россия
\.


--
-- TOC entry 3001 (class 0 OID 16438)
-- Dependencies: 203
-- Data for Name: flight; Type: TABLE DATA; Schema: public; Owner: program
--

COPY public.flight (id, flight_number, datetime, from_airport_id, to_airport_id, price) FROM stdin;
1	AFL031	2021-10-08 20:00:00+00	1	2	1500
\.


--
-- TOC entry 3009 (class 0 OID 0)
-- Dependencies: 200
-- Name: airport_id_seq; Type: SEQUENCE SET; Schema: public; Owner: program
--

SELECT pg_catalog.setval('public.airport_id_seq', 1, false);


--
-- TOC entry 3010 (class 0 OID 0)
-- Dependencies: 202
-- Name: flight_id_seq; Type: SEQUENCE SET; Schema: public; Owner: program
--

SELECT pg_catalog.setval('public.flight_id_seq', 1, false);


--
-- TOC entry 2863 (class 2606 OID 16435)
-- Name: airport airport_pkey; Type: CONSTRAINT; Schema: public; Owner: program
--

ALTER TABLE ONLY public.airport
    ADD CONSTRAINT airport_pkey PRIMARY KEY (id);


--
-- TOC entry 2865 (class 2606 OID 16443)
-- Name: flight flight_pkey; Type: CONSTRAINT; Schema: public; Owner: program
--

ALTER TABLE ONLY public.flight
    ADD CONSTRAINT flight_pkey PRIMARY KEY (id);


--
-- TOC entry 2866 (class 2606 OID 16444)
-- Name: flight flight_from_airport_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: program
--

ALTER TABLE ONLY public.flight
    ADD CONSTRAINT flight_from_airport_id_fkey FOREIGN KEY (from_airport_id) REFERENCES public.airport(id);


--
-- TOC entry 2867 (class 2606 OID 16449)
-- Name: flight flight_to_airport_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: program
--

ALTER TABLE ONLY public.flight
    ADD CONSTRAINT flight_to_airport_id_fkey FOREIGN KEY (to_airport_id) REFERENCES public.airport(id);


-- Completed on 2022-11-06 15:00:28 MSK

--
-- PostgreSQL database dump complete
--

