PGDMP  +    +                 }         	   Extrusion    16.4 (Debian 16.4-1.pgdg120+1)    16.4 �    �           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            �           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            �           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            �           1262    16384 	   Extrusion    DATABASE     v   CREATE DATABASE "Extrusion" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'en_US.utf8';
    DROP DATABASE "Extrusion";
                postgres    false            �            1259    16389    Barrel    TABLE     r   CREATE TABLE public."Barrel" (
    id bigint NOT NULL,
    name text NOT NULL
)
WITH (autovacuum_enabled='true');
    DROP TABLE public."Barrel";
       public         heap    postgres    false            �            1259    16394    BarrelParametr    TABLE     �   CREATE TABLE public."BarrelParametr" (
    id bigint NOT NULL,
    id_unit bigint NOT NULL,
    designation text NOT NULL,
    name text NOT NULL
)
WITH (autovacuum_enabled='true');
 $   DROP TABLE public."BarrelParametr";
       public         heap    postgres    false            �            1259    16399    BarrelParametrValue    TABLE     �   CREATE TABLE public."BarrelParametrValue" (
    id_barrel bigint NOT NULL,
    id_parametr bigint NOT NULL,
    value double precision NOT NULL
)
WITH (autovacuum_enabled='true');
 )   DROP TABLE public."BarrelParametrValue";
       public         heap    postgres    false            �            1259    16402    BarrelParametr_id_seq    SEQUENCE     �   ALTER TABLE public."BarrelParametr" ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."BarrelParametr_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    216            �            1259    16403    BarrelPossibleСonfiguration    TABLE     �   CREATE TABLE public."BarrelPossibleСonfiguration" (
    id bigint NOT NULL,
    id_body bigint NOT NULL,
    id_configuration bigint
)
WITH (autovacuum_enabled='true');
 2   DROP TABLE public."BarrelPossibleСonfiguration";
       public         heap    postgres    false            �            1259    16406 #   BarrelPossibleСonfiguration_id_seq    SEQUENCE     �   ALTER TABLE public."BarrelPossibleСonfiguration" ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."BarrelPossibleСonfiguration_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    219            �            1259    16407    BarrelSection    TABLE     y   CREATE TABLE public."BarrelSection" (
    id bigint NOT NULL,
    name text NOT NULL
)
WITH (autovacuum_enabled='true');
 #   DROP TABLE public."BarrelSection";
       public         heap    postgres    false            �            1259    16412    BarrelSectionInСonfiguration    TABLE     �   CREATE TABLE public."BarrelSectionInСonfiguration" (
    id bigint NOT NULL,
    id_configuration bigint NOT NULL,
    id_element bigint NOT NULL,
    number smallint NOT NULL
)
WITH (autovacuum_enabled='true');
 3   DROP TABLE public."BarrelSectionInСonfiguration";
       public         heap    postgres    false            �            1259    16415 $   BarrelSectionInСonfiguration_id_seq    SEQUENCE     �   ALTER TABLE public."BarrelSectionInСonfiguration" ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."BarrelSectionInСonfiguration_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    222            �            1259    16416    BarrelSectionParametr    TABLE     �   CREATE TABLE public."BarrelSectionParametr" (
    id bigint NOT NULL,
    id_unit bigint NOT NULL,
    designation text NOT NULL,
    name text NOT NULL
)
WITH (autovacuum_enabled='true');
 +   DROP TABLE public."BarrelSectionParametr";
       public         heap    postgres    false            �            1259    16421    BarrelSectionParametrValue    TABLE     �   CREATE TABLE public."BarrelSectionParametrValue" (
    id_element bigint NOT NULL,
    id_parametr bigint NOT NULL,
    value double precision NOT NULL
)
WITH (autovacuum_enabled='true');
 0   DROP TABLE public."BarrelSectionParametrValue";
       public         heap    postgres    false            �            1259    16424    BarrelSectionParametr_id_seq    SEQUENCE     �   ALTER TABLE public."BarrelSectionParametr" ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."BarrelSectionParametr_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    224            �            1259    16425    BarrelSection_id_seq    SEQUENCE     �   ALTER TABLE public."BarrelSection" ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."BarrelSection_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    221            �            1259    16426    Barrel_id_seq    SEQUENCE     �   ALTER TABLE public."Barrel" ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Barrel_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    215            �            1259    16427    BarrelСonfiguration    TABLE     �   CREATE TABLE public."BarrelСonfiguration" (
    id bigint NOT NULL,
    name text NOT NULL
)
WITH (autovacuum_enabled='true');
 *   DROP TABLE public."BarrelСonfiguration";
       public         heap    postgres    false            �            1259    16432    BarrelСonfiguration_id_seq    SEQUENCE     �   ALTER TABLE public."BarrelСonfiguration" ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."BarrelСonfiguration_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    229                       1259    16893    ColorInterval    TABLE     
  CREATE TABLE public."ColorInterval" (
    id bigint NOT NULL,
    film_id bigint NOT NULL,
    "min_delE" real NOT NULL,
    "max_delE" real NOT NULL,
    is_base_color boolean DEFAULT false NOT NULL,
    l real NOT NULL,
    a real NOT NULL,
    b real NOT NULL
);
 #   DROP TABLE public."ColorInterval";
       public         heap    postgres    false                       1259    16892    ColorInterval_id_seq    SEQUENCE     �   ALTER TABLE public."ColorInterval" ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."ColorInterval_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    280            �            1259    16433    Die    TABLE     o   CREATE TABLE public."Die" (
    id bigint NOT NULL,
    name text NOT NULL
)
WITH (autovacuum_enabled='true');
    DROP TABLE public."Die";
       public         heap    postgres    false            �            1259    16438 
   DieElement    TABLE     v   CREATE TABLE public."DieElement" (
    id bigint NOT NULL,
    name text NOT NULL
)
WITH (autovacuum_enabled='true');
     DROP TABLE public."DieElement";
       public         heap    postgres    false            �            1259    16443    DieElementInСonfiguration    TABLE     �   CREATE TABLE public."DieElementInСonfiguration" (
    id bigint NOT NULL,
    id_die bigint NOT NULL,
    id_element bigint NOT NULL,
    number smallint NOT NULL
)
WITH (autovacuum_enabled='true');
 0   DROP TABLE public."DieElementInСonfiguration";
       public         heap    postgres    false            �            1259    16446 !   DieElementInСonfiguration_id_seq    SEQUENCE     �   ALTER TABLE public."DieElementInСonfiguration" ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."DieElementInСonfiguration_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    233            �            1259    16447    DieElementParametr    TABLE     �   CREATE TABLE public."DieElementParametr" (
    id bigint NOT NULL,
    id_unit bigint NOT NULL,
    designation text NOT NULL,
    name text NOT NULL
)
WITH (autovacuum_enabled='true');
 (   DROP TABLE public."DieElementParametr";
       public         heap    postgres    false            �            1259    16452    DieElementParametrValue    TABLE     �   CREATE TABLE public."DieElementParametrValue" (
    id_element bigint NOT NULL,
    id_parametr bigint NOT NULL,
    value double precision NOT NULL
)
WITH (autovacuum_enabled='true');
 -   DROP TABLE public."DieElementParametrValue";
       public         heap    postgres    false            �            1259    16455    DieElementParametr_id_seq    SEQUENCE     �   ALTER TABLE public."DieElementParametr" ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."DieElementParametr_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    235            �            1259    16456    DieElement_id_seq    SEQUENCE     �   ALTER TABLE public."DieElement" ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."DieElement_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    232            �            1259    16457 
   Die_id_seq    SEQUENCE     �   ALTER TABLE public."Die" ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Die_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    231            �            1259    16458    Extruder    TABLE       CREATE TABLE public."Extruder" (
    id bigint NOT NULL,
    id_type bigint NOT NULL,
    id_die bigint NOT NULL,
    id_screw1 bigint NOT NULL,
    id_screw2 bigint,
    id_barrel bigint NOT NULL,
    brand text NOT NULL
)
WITH (autovacuum_enabled='true');
    DROP TABLE public."Extruder";
       public         heap    postgres    false            �            1259    16463    ExtruderType    TABLE     �   CREATE TABLE public."ExtruderType" (
    id bigint NOT NULL,
    id_model bigint,
    name text NOT NULL
)
WITH (autovacuum_enabled='true');
 "   DROP TABLE public."ExtruderType";
       public         heap    postgres    false            �            1259    16468    ExtruderType_id_seq    SEQUENCE     �   ALTER TABLE public."ExtruderType" ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."ExtruderType_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    241            �            1259    16469    Extruder_id_seq    SEQUENCE     �   ALTER TABLE public."Extruder" ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Extruder_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    240            �            1259    16470    Film    TABLE     �   CREATE TABLE public."Film" (
    id bigint NOT NULL,
    id_polymer bigint,
    type text NOT NULL,
    "max_delE" double precision DEFAULT 0 NOT NULL
)
WITH (autovacuum_enabled='true');
    DROP TABLE public."Film";
       public         heap    postgres    false            �            1259    16475    Film_id_seq    SEQUENCE     �   ALTER TABLE public."Film" ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Film_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    244            �            1259    16476 	   MathModel    TABLE     u   CREATE TABLE public."MathModel" (
    id bigint NOT NULL,
    name text NOT NULL
)
WITH (autovacuum_enabled='true');
    DROP TABLE public."MathModel";
       public         heap    postgres    false            �            1259    16481    MathModelCoefficient    TABLE     �   CREATE TABLE public."MathModelCoefficient" (
    id bigint NOT NULL,
    id_unit bigint NOT NULL,
    designation text NOT NULL,
    name text NOT NULL
)
WITH (autovacuum_enabled='true');
 *   DROP TABLE public."MathModelCoefficient";
       public         heap    postgres    false            �            1259    16486    MathModelCoefficientValue    TABLE     �   CREATE TABLE public."MathModelCoefficientValue" (
    id_model bigint NOT NULL,
    id_coefficient bigint NOT NULL,
    id_polymer bigint NOT NULL,
    value double precision NOT NULL
)
WITH (autovacuum_enabled='true');
 /   DROP TABLE public."MathModelCoefficientValue";
       public         heap    postgres    false            �            1259    16489    MathModelCoefficient_id_seq    SEQUENCE     �   ALTER TABLE public."MathModelCoefficient" ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."MathModelCoefficient_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    247            �            1259    16490    MathModel_id_seq    SEQUENCE     �   ALTER TABLE public."MathModel" ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."MathModel_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    246            �            1259    16491    Polymer    TABLE     s   CREATE TABLE public."Polymer" (
    id bigint NOT NULL,
    name text NOT NULL
)
WITH (autovacuum_enabled='true');
    DROP TABLE public."Polymer";
       public         heap    postgres    false            �            1259    16496    PolymerParametr    TABLE     �   CREATE TABLE public."PolymerParametr" (
    id bigint NOT NULL,
    id_unit bigint NOT NULL,
    designation text NOT NULL,
    name text NOT NULL
)
WITH (autovacuum_enabled='true');
 %   DROP TABLE public."PolymerParametr";
       public         heap    postgres    false            �            1259    16501    PolymerParametrValue    TABLE     �   CREATE TABLE public."PolymerParametrValue" (
    id_polymer bigint NOT NULL,
    id_parametr bigint NOT NULL,
    value double precision NOT NULL
)
WITH (autovacuum_enabled='true');
 *   DROP TABLE public."PolymerParametrValue";
       public         heap    postgres    false            �            1259    16504    PolymerParametr_id_seq    SEQUENCE     �   ALTER TABLE public."PolymerParametr" ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."PolymerParametr_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    252            �            1259    16505    Polymer_id_seq    SEQUENCE     �   ALTER TABLE public."Polymer" ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Polymer_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    251                        1259    16506    ProcessParametr    TABLE     �   CREATE TABLE public."ProcessParametr" (
    id bigint NOT NULL,
    id_unit bigint NOT NULL,
    designation text NOT NULL,
    name text NOT NULL
)
WITH (autovacuum_enabled='true');
 %   DROP TABLE public."ProcessParametr";
       public         heap    postgres    false                       1259    16511    ProcessParametrValue    TABLE     �   CREATE TABLE public."ProcessParametrValue" (
    id_film bigint NOT NULL,
    id_parametr bigint NOT NULL,
    min_value double precision,
    max_value double precision
)
WITH (autovacuum_enabled='true');
 *   DROP TABLE public."ProcessParametrValue";
       public         heap    postgres    false                       1259    16514    ProcessParametr_id_seq    SEQUENCE     �   ALTER TABLE public."ProcessParametr" ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."ProcessParametr_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    256                       1259    16515    Scenario    TABLE     �   CREATE TABLE public."Scenario" (
    id bigint NOT NULL,
    id_film bigint NOT NULL,
    id_extruder bigint NOT NULL,
    name text NOT NULL,
    throughput double precision NOT NULL,
    "time" integer NOT NULL
)
WITH (autovacuum_enabled='true');
    DROP TABLE public."Scenario";
       public         heap    postgres    false                       1259    16520    Scenario_id_seq    SEQUENCE     �   ALTER TABLE public."Scenario" ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Scenario_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    259                       1259    16521    Screw    TABLE     q   CREATE TABLE public."Screw" (
    id bigint NOT NULL,
    name text NOT NULL
)
WITH (autovacuum_enabled='true');
    DROP TABLE public."Screw";
       public         heap    postgres    false                       1259    16526    ScrewElement    TABLE     x   CREATE TABLE public."ScrewElement" (
    id bigint NOT NULL,
    name text NOT NULL
)
WITH (autovacuum_enabled='true');
 "   DROP TABLE public."ScrewElement";
       public         heap    postgres    false                       1259    16531    ScrewElementInСonfiguration    TABLE     �   CREATE TABLE public."ScrewElementInСonfiguration" (
    id bigint NOT NULL,
    id_configuration bigint NOT NULL,
    id_element bigint NOT NULL,
    number smallint NOT NULL
)
WITH (autovacuum_enabled='true');
 2   DROP TABLE public."ScrewElementInСonfiguration";
       public         heap    postgres    false                       1259    16534 #   ScrewElementInСonfiguration_id_seq    SEQUENCE     �   ALTER TABLE public."ScrewElementInСonfiguration" ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."ScrewElementInСonfiguration_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    263            	           1259    16535    ScrewElementParametr    TABLE     �   CREATE TABLE public."ScrewElementParametr" (
    id bigint NOT NULL,
    id_unit bigint NOT NULL,
    designation text NOT NULL,
    name text NOT NULL
)
WITH (autovacuum_enabled='true');
 *   DROP TABLE public."ScrewElementParametr";
       public         heap    postgres    false            
           1259    16540    ScrewElementParametrValue    TABLE     �   CREATE TABLE public."ScrewElementParametrValue" (
    id_element bigint NOT NULL,
    id_parametr bigint NOT NULL,
    value double precision NOT NULL
)
WITH (autovacuum_enabled='true');
 /   DROP TABLE public."ScrewElementParametrValue";
       public         heap    postgres    false                       1259    16543    ScrewElementParametr_id_seq    SEQUENCE     �   ALTER TABLE public."ScrewElementParametr" ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."ScrewElementParametr_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    265                       1259    16544    ScrewElement_id_seq    SEQUENCE     �   ALTER TABLE public."ScrewElement" ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."ScrewElement_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    262                       1259    16545    ScrewParametr    TABLE     �   CREATE TABLE public."ScrewParametr" (
    id bigint NOT NULL,
    id_unit bigint NOT NULL,
    designation text NOT NULL,
    name text NOT NULL
)
WITH (autovacuum_enabled='true');
 #   DROP TABLE public."ScrewParametr";
       public         heap    postgres    false                       1259    16550    ScrewParametrValue    TABLE     �   CREATE TABLE public."ScrewParametrValue" (
    id_screw bigint NOT NULL,
    id_parametr bigint NOT NULL,
    value double precision NOT NULL
)
WITH (autovacuum_enabled='true');
 (   DROP TABLE public."ScrewParametrValue";
       public         heap    postgres    false                       1259    16553    ScrewParametr_id_seq    SEQUENCE     �   ALTER TABLE public."ScrewParametr" ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."ScrewParametr_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    269                       1259    16554    ScrewPossibleСonfiguration    TABLE     �   CREATE TABLE public."ScrewPossibleСonfiguration" (
    id bigint NOT NULL,
    id_screw bigint NOT NULL,
    id_configuration bigint NOT NULL
)
WITH (autovacuum_enabled='true');
 1   DROP TABLE public."ScrewPossibleСonfiguration";
       public         heap    postgres    false                       1259    16557 "   ScrewPossibleСonfiguration_id_seq    SEQUENCE     �   ALTER TABLE public."ScrewPossibleСonfiguration" ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."ScrewPossibleСonfiguration_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    272                       1259    16558    Screw_id_seq    SEQUENCE     �   ALTER TABLE public."Screw" ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Screw_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    261                       1259    16559    ScrewСonfiguration    TABLE        CREATE TABLE public."ScrewСonfiguration" (
    id bigint NOT NULL,
    name text NOT NULL
)
WITH (autovacuum_enabled='true');
 )   DROP TABLE public."ScrewСonfiguration";
       public         heap    postgres    false                       1259    16564    ScrewСonfiguration_id_seq    SEQUENCE     �   ALTER TABLE public."ScrewСonfiguration" ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."ScrewСonfiguration_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    275                       1259    16565    Unit    TABLE     w   CREATE TABLE public."Unit" (
    id bigint NOT NULL,
    designation text NOT NULL
)
WITH (autovacuum_enabled='true');
    DROP TABLE public."Unit";
       public         heap    postgres    false                       1259    16570    Unit_id_seq    SEQUENCE     �   ALTER TABLE public."Unit" ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Unit_id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    277            S          0    16389    Barrel 
   TABLE DATA           ,   COPY public."Barrel" (id, name) FROM stdin;
    public          postgres    false    215   d1      T          0    16394    BarrelParametr 
   TABLE DATA           J   COPY public."BarrelParametr" (id, id_unit, designation, name) FROM stdin;
    public          postgres    false    216   �1      U          0    16399    BarrelParametrValue 
   TABLE DATA           N   COPY public."BarrelParametrValue" (id_barrel, id_parametr, value) FROM stdin;
    public          postgres    false    217   �1      W          0    16403    BarrelPossibleСonfiguration 
   TABLE DATA           W   COPY public."BarrelPossibleСonfiguration" (id, id_body, id_configuration) FROM stdin;
    public          postgres    false    219   �1      Y          0    16407    BarrelSection 
   TABLE DATA           3   COPY public."BarrelSection" (id, name) FROM stdin;
    public          postgres    false    221   2      Z          0    16412    BarrelSectionInСonfiguration 
   TABLE DATA           c   COPY public."BarrelSectionInСonfiguration" (id, id_configuration, id_element, number) FROM stdin;
    public          postgres    false    222   ^2      \          0    16416    BarrelSectionParametr 
   TABLE DATA           Q   COPY public."BarrelSectionParametr" (id, id_unit, designation, name) FROM stdin;
    public          postgres    false    224   �2      ]          0    16421    BarrelSectionParametrValue 
   TABLE DATA           V   COPY public."BarrelSectionParametrValue" (id_element, id_parametr, value) FROM stdin;
    public          postgres    false    225   3      a          0    16427    BarrelСonfiguration 
   TABLE DATA           :   COPY public."BarrelСonfiguration" (id, name) FROM stdin;
    public          postgres    false    229   �3      �          0    16893    ColorInterval 
   TABLE DATA           f   COPY public."ColorInterval" (id, film_id, "min_delE", "max_delE", is_base_color, l, a, b) FROM stdin;
    public          postgres    false    280   �3      c          0    16433    Die 
   TABLE DATA           )   COPY public."Die" (id, name) FROM stdin;
    public          postgres    false    231   �4      d          0    16438 
   DieElement 
   TABLE DATA           0   COPY public."DieElement" (id, name) FROM stdin;
    public          postgres    false    232   �4      e          0    16443    DieElementInСonfiguration 
   TABLE DATA           V   COPY public."DieElementInСonfiguration" (id, id_die, id_element, number) FROM stdin;
    public          postgres    false    233   5      g          0    16447    DieElementParametr 
   TABLE DATA           N   COPY public."DieElementParametr" (id, id_unit, designation, name) FROM stdin;
    public          postgres    false    235   c5      h          0    16452    DieElementParametrValue 
   TABLE DATA           S   COPY public."DieElementParametrValue" (id_element, id_parametr, value) FROM stdin;
    public          postgres    false    236   �5      l          0    16458    Extruder 
   TABLE DATA           a   COPY public."Extruder" (id, id_type, id_die, id_screw1, id_screw2, id_barrel, brand) FROM stdin;
    public          postgres    false    240   �6      m          0    16463    ExtruderType 
   TABLE DATA           <   COPY public."ExtruderType" (id, id_model, name) FROM stdin;
    public          postgres    false    241   �6      p          0    16470    Film 
   TABLE DATA           B   COPY public."Film" (id, id_polymer, type, "max_delE") FROM stdin;
    public          postgres    false    244   "7      r          0    16476 	   MathModel 
   TABLE DATA           /   COPY public."MathModel" (id, name) FROM stdin;
    public          postgres    false    246   e7      s          0    16481    MathModelCoefficient 
   TABLE DATA           P   COPY public."MathModelCoefficient" (id, id_unit, designation, name) FROM stdin;
    public          postgres    false    247   �7      t          0    16486    MathModelCoefficientValue 
   TABLE DATA           b   COPY public."MathModelCoefficientValue" (id_model, id_coefficient, id_polymer, value) FROM stdin;
    public          postgres    false    248   �7      w          0    16491    Polymer 
   TABLE DATA           -   COPY public."Polymer" (id, name) FROM stdin;
    public          postgres    false    251   �7      x          0    16496    PolymerParametr 
   TABLE DATA           K   COPY public."PolymerParametr" (id, id_unit, designation, name) FROM stdin;
    public          postgres    false    252   �7      y          0    16501    PolymerParametrValue 
   TABLE DATA           P   COPY public."PolymerParametrValue" (id_polymer, id_parametr, value) FROM stdin;
    public          postgres    false    253   w8      |          0    16506    ProcessParametr 
   TABLE DATA           K   COPY public."ProcessParametr" (id, id_unit, designation, name) FROM stdin;
    public          postgres    false    256    9      }          0    16511    ProcessParametrValue 
   TABLE DATA           \   COPY public."ProcessParametrValue" (id_film, id_parametr, min_value, max_value) FROM stdin;
    public          postgres    false    257   W9                0    16515    Scenario 
   TABLE DATA           X   COPY public."Scenario" (id, id_film, id_extruder, name, throughput, "time") FROM stdin;
    public          postgres    false    259   �9      �          0    16521    Screw 
   TABLE DATA           +   COPY public."Screw" (id, name) FROM stdin;
    public          postgres    false    261   �9      �          0    16526    ScrewElement 
   TABLE DATA           2   COPY public."ScrewElement" (id, name) FROM stdin;
    public          postgres    false    262   :      �          0    16531    ScrewElementInСonfiguration 
   TABLE DATA           b   COPY public."ScrewElementInСonfiguration" (id, id_configuration, id_element, number) FROM stdin;
    public          postgres    false    263   o:      �          0    16535    ScrewElementParametr 
   TABLE DATA           P   COPY public."ScrewElementParametr" (id, id_unit, designation, name) FROM stdin;
    public          postgres    false    265   �:      �          0    16540    ScrewElementParametrValue 
   TABLE DATA           U   COPY public."ScrewElementParametrValue" (id_element, id_parametr, value) FROM stdin;
    public          postgres    false    266   T;      �          0    16545    ScrewParametr 
   TABLE DATA           I   COPY public."ScrewParametr" (id, id_unit, designation, name) FROM stdin;
    public          postgres    false    269   �<      �          0    16550    ScrewParametrValue 
   TABLE DATA           L   COPY public."ScrewParametrValue" (id_screw, id_parametr, value) FROM stdin;
    public          postgres    false    270   i=      �          0    16554    ScrewPossibleСonfiguration 
   TABLE DATA           W   COPY public."ScrewPossibleСonfiguration" (id, id_screw, id_configuration) FROM stdin;
    public          postgres    false    272   �=      �          0    16559    ScrewСonfiguration 
   TABLE DATA           9   COPY public."ScrewСonfiguration" (id, name) FROM stdin;
    public          postgres    false    275   ">      �          0    16565    Unit 
   TABLE DATA           1   COPY public."Unit" (id, designation) FROM stdin;
    public          postgres    false    277   Z>      �           0    0    BarrelParametr_id_seq    SEQUENCE SET     E   SELECT pg_catalog.setval('public."BarrelParametr_id_seq"', 1, true);
          public          postgres    false    218            �           0    0 #   BarrelPossibleСonfiguration_id_seq    SEQUENCE SET     S   SELECT pg_catalog.setval('public."BarrelPossibleСonfiguration_id_seq"', 6, true);
          public          postgres    false    220            �           0    0 $   BarrelSectionInСonfiguration_id_seq    SEQUENCE SET     U   SELECT pg_catalog.setval('public."BarrelSectionInСonfiguration_id_seq"', 19, true);
          public          postgres    false    223            �           0    0    BarrelSectionParametr_id_seq    SEQUENCE SET     L   SELECT pg_catalog.setval('public."BarrelSectionParametr_id_seq"', 8, true);
          public          postgres    false    226            �           0    0    BarrelSection_id_seq    SEQUENCE SET     D   SELECT pg_catalog.setval('public."BarrelSection_id_seq"', 7, true);
          public          postgres    false    227            �           0    0    Barrel_id_seq    SEQUENCE SET     =   SELECT pg_catalog.setval('public."Barrel_id_seq"', 6, true);
          public          postgres    false    228            �           0    0    BarrelСonfiguration_id_seq    SEQUENCE SET     K   SELECT pg_catalog.setval('public."BarrelСonfiguration_id_seq"', 6, true);
          public          postgres    false    230            �           0    0    ColorInterval_id_seq    SEQUENCE SET     E   SELECT pg_catalog.setval('public."ColorInterval_id_seq"', 26, true);
          public          postgres    false    279            �           0    0 !   DieElementInСonfiguration_id_seq    SEQUENCE SET     R   SELECT pg_catalog.setval('public."DieElementInСonfiguration_id_seq"', 17, true);
          public          postgres    false    234            �           0    0    DieElementParametr_id_seq    SEQUENCE SET     I   SELECT pg_catalog.setval('public."DieElementParametr_id_seq"', 8, true);
          public          postgres    false    237            �           0    0    DieElement_id_seq    SEQUENCE SET     B   SELECT pg_catalog.setval('public."DieElement_id_seq"', 11, true);
          public          postgres    false    238            �           0    0 
   Die_id_seq    SEQUENCE SET     ;   SELECT pg_catalog.setval('public."Die_id_seq"', 10, true);
          public          postgres    false    239            �           0    0    ExtruderType_id_seq    SEQUENCE SET     C   SELECT pg_catalog.setval('public."ExtruderType_id_seq"', 1, true);
          public          postgres    false    242            �           0    0    Extruder_id_seq    SEQUENCE SET     @   SELECT pg_catalog.setval('public."Extruder_id_seq"', 12, true);
          public          postgres    false    243            �           0    0    Film_id_seq    SEQUENCE SET     <   SELECT pg_catalog.setval('public."Film_id_seq"', 11, true);
          public          postgres    false    245            �           0    0    MathModelCoefficient_id_seq    SEQUENCE SET     L   SELECT pg_catalog.setval('public."MathModelCoefficient_id_seq"', 1, false);
          public          postgres    false    249            �           0    0    MathModel_id_seq    SEQUENCE SET     @   SELECT pg_catalog.setval('public."MathModel_id_seq"', 1, true);
          public          postgres    false    250            �           0    0    PolymerParametr_id_seq    SEQUENCE SET     G   SELECT pg_catalog.setval('public."PolymerParametr_id_seq"', 13, true);
          public          postgres    false    254            �           0    0    Polymer_id_seq    SEQUENCE SET     >   SELECT pg_catalog.setval('public."Polymer_id_seq"', 5, true);
          public          postgres    false    255            �           0    0    ProcessParametr_id_seq    SEQUENCE SET     F   SELECT pg_catalog.setval('public."ProcessParametr_id_seq"', 3, true);
          public          postgres    false    258            �           0    0    Scenario_id_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('public."Scenario_id_seq"', 7, true);
          public          postgres    false    260            �           0    0 #   ScrewElementInСonfiguration_id_seq    SEQUENCE SET     T   SELECT pg_catalog.setval('public."ScrewElementInСonfiguration_id_seq"', 11, true);
          public          postgres    false    264            �           0    0    ScrewElementParametr_id_seq    SEQUENCE SET     L   SELECT pg_catalog.setval('public."ScrewElementParametr_id_seq"', 15, true);
          public          postgres    false    267            �           0    0    ScrewElement_id_seq    SEQUENCE SET     D   SELECT pg_catalog.setval('public."ScrewElement_id_seq"', 10, true);
          public          postgres    false    268            �           0    0    ScrewParametr_id_seq    SEQUENCE SET     E   SELECT pg_catalog.setval('public."ScrewParametr_id_seq"', 14, true);
          public          postgres    false    271            �           0    0 "   ScrewPossibleСonfiguration_id_seq    SEQUENCE SET     R   SELECT pg_catalog.setval('public."ScrewPossibleСonfiguration_id_seq"', 3, true);
          public          postgres    false    273            �           0    0    Screw_id_seq    SEQUENCE SET     <   SELECT pg_catalog.setval('public."Screw_id_seq"', 3, true);
          public          postgres    false    274            �           0    0    ScrewСonfiguration_id_seq    SEQUENCE SET     J   SELECT pg_catalog.setval('public."ScrewСonfiguration_id_seq"', 3, true);
          public          postgres    false    276            �           0    0    Unit_id_seq    SEQUENCE SET     ;   SELECT pg_catalog.setval('public."Unit_id_seq"', 1, true);
          public          postgres    false    278            2           2606    16572    Barrel PK_Barrel 
   CONSTRAINT     R   ALTER TABLE ONLY public."Barrel"
    ADD CONSTRAINT "PK_Barrel" PRIMARY KEY (id);
 >   ALTER TABLE ONLY public."Barrel" DROP CONSTRAINT "PK_Barrel";
       public            postgres    false    215            5           2606    16574     BarrelParametr PK_BarrelParametr 
   CONSTRAINT     b   ALTER TABLE ONLY public."BarrelParametr"
    ADD CONSTRAINT "PK_BarrelParametr" PRIMARY KEY (id);
 N   ALTER TABLE ONLY public."BarrelParametr" DROP CONSTRAINT "PK_BarrelParametr";
       public            postgres    false    216            7           2606    16576 *   BarrelParametrValue PK_BarrelParametrValue 
   CONSTRAINT     �   ALTER TABLE ONLY public."BarrelParametrValue"
    ADD CONSTRAINT "PK_BarrelParametrValue" PRIMARY KEY (id_barrel, id_parametr);
 X   ALTER TABLE ONLY public."BarrelParametrValue" DROP CONSTRAINT "PK_BarrelParametrValue";
       public            postgres    false    217    217            ;           2606    16578 <   BarrelPossibleСonfiguration PK_BarrelPossibleСonfiguration 
   CONSTRAINT     ~   ALTER TABLE ONLY public."BarrelPossibleСonfiguration"
    ADD CONSTRAINT "PK_BarrelPossibleСonfiguration" PRIMARY KEY (id);
 j   ALTER TABLE ONLY public."BarrelPossibleСonfiguration" DROP CONSTRAINT "PK_BarrelPossibleСonfiguration";
       public            postgres    false    219            =           2606    16580    BarrelSection PK_BarrelSection 
   CONSTRAINT     `   ALTER TABLE ONLY public."BarrelSection"
    ADD CONSTRAINT "PK_BarrelSection" PRIMARY KEY (id);
 L   ALTER TABLE ONLY public."BarrelSection" DROP CONSTRAINT "PK_BarrelSection";
       public            postgres    false    221            A           2606    16582 >   BarrelSectionInСonfiguration PK_BarrelSectionInСonfiguration 
   CONSTRAINT     �   ALTER TABLE ONLY public."BarrelSectionInСonfiguration"
    ADD CONSTRAINT "PK_BarrelSectionInСonfiguration" PRIMARY KEY (id);
 l   ALTER TABLE ONLY public."BarrelSectionInСonfiguration" DROP CONSTRAINT "PK_BarrelSectionInСonfiguration";
       public            postgres    false    222            D           2606    16584 .   BarrelSectionParametr PK_BarrelSectionParametr 
   CONSTRAINT     p   ALTER TABLE ONLY public."BarrelSectionParametr"
    ADD CONSTRAINT "PK_BarrelSectionParametr" PRIMARY KEY (id);
 \   ALTER TABLE ONLY public."BarrelSectionParametr" DROP CONSTRAINT "PK_BarrelSectionParametr";
       public            postgres    false    224            F           2606    16586 8   BarrelSectionParametrValue PK_BarrelSectionParametrValue 
   CONSTRAINT     �   ALTER TABLE ONLY public."BarrelSectionParametrValue"
    ADD CONSTRAINT "PK_BarrelSectionParametrValue" PRIMARY KEY (id_parametr, id_element);
 f   ALTER TABLE ONLY public."BarrelSectionParametrValue" DROP CONSTRAINT "PK_BarrelSectionParametrValue";
       public            postgres    false    225    225            H           2606    16588 ,   BarrelСonfiguration PK_BarrelСonfiguration 
   CONSTRAINT     n   ALTER TABLE ONLY public."BarrelСonfiguration"
    ADD CONSTRAINT "PK_BarrelСonfiguration" PRIMARY KEY (id);
 Z   ALTER TABLE ONLY public."BarrelСonfiguration" DROP CONSTRAINT "PK_BarrelСonfiguration";
       public            postgres    false    229            �           2606    16898    ColorInterval PK_ColorInterval 
   CONSTRAINT     `   ALTER TABLE ONLY public."ColorInterval"
    ADD CONSTRAINT "PK_ColorInterval" PRIMARY KEY (id);
 L   ALTER TABLE ONLY public."ColorInterval" DROP CONSTRAINT "PK_ColorInterval";
       public            postgres    false    280            J           2606    16590 
   Die PK_Die 
   CONSTRAINT     L   ALTER TABLE ONLY public."Die"
    ADD CONSTRAINT "PK_Die" PRIMARY KEY (id);
 8   ALTER TABLE ONLY public."Die" DROP CONSTRAINT "PK_Die";
       public            postgres    false    231            L           2606    16592    DieElement PK_DieElement 
   CONSTRAINT     Z   ALTER TABLE ONLY public."DieElement"
    ADD CONSTRAINT "PK_DieElement" PRIMARY KEY (id);
 F   ALTER TABLE ONLY public."DieElement" DROP CONSTRAINT "PK_DieElement";
       public            postgres    false    232            P           2606    16594 8   DieElementInСonfiguration PK_DieElementInСonfiguration 
   CONSTRAINT     z   ALTER TABLE ONLY public."DieElementInСonfiguration"
    ADD CONSTRAINT "PK_DieElementInСonfiguration" PRIMARY KEY (id);
 f   ALTER TABLE ONLY public."DieElementInСonfiguration" DROP CONSTRAINT "PK_DieElementInСonfiguration";
       public            postgres    false    233            S           2606    16596 (   DieElementParametr PK_DieElementParametr 
   CONSTRAINT     j   ALTER TABLE ONLY public."DieElementParametr"
    ADD CONSTRAINT "PK_DieElementParametr" PRIMARY KEY (id);
 V   ALTER TABLE ONLY public."DieElementParametr" DROP CONSTRAINT "PK_DieElementParametr";
       public            postgres    false    235            U           2606    16598 2   DieElementParametrValue PK_DieElementParametrValue 
   CONSTRAINT     �   ALTER TABLE ONLY public."DieElementParametrValue"
    ADD CONSTRAINT "PK_DieElementParametrValue" PRIMARY KEY (id_parametr, id_element);
 `   ALTER TABLE ONLY public."DieElementParametrValue" DROP CONSTRAINT "PK_DieElementParametrValue";
       public            postgres    false    236    236            \           2606    16600    Extruder PK_Extruder 
   CONSTRAINT     V   ALTER TABLE ONLY public."Extruder"
    ADD CONSTRAINT "PK_Extruder" PRIMARY KEY (id);
 B   ALTER TABLE ONLY public."Extruder" DROP CONSTRAINT "PK_Extruder";
       public            postgres    false    240            _           2606    16602    ExtruderType PK_ExtruderType 
   CONSTRAINT     ^   ALTER TABLE ONLY public."ExtruderType"
    ADD CONSTRAINT "PK_ExtruderType" PRIMARY KEY (id);
 J   ALTER TABLE ONLY public."ExtruderType" DROP CONSTRAINT "PK_ExtruderType";
       public            postgres    false    241            b           2606    16604    Film PK_Film 
   CONSTRAINT     N   ALTER TABLE ONLY public."Film"
    ADD CONSTRAINT "PK_Film" PRIMARY KEY (id);
 :   ALTER TABLE ONLY public."Film" DROP CONSTRAINT "PK_Film";
       public            postgres    false    244            d           2606    16606    MathModel PK_MathModel 
   CONSTRAINT     X   ALTER TABLE ONLY public."MathModel"
    ADD CONSTRAINT "PK_MathModel" PRIMARY KEY (id);
 D   ALTER TABLE ONLY public."MathModel" DROP CONSTRAINT "PK_MathModel";
       public            postgres    false    246            g           2606    16608 ,   MathModelCoefficient PK_MathModelCoefficient 
   CONSTRAINT     n   ALTER TABLE ONLY public."MathModelCoefficient"
    ADD CONSTRAINT "PK_MathModelCoefficient" PRIMARY KEY (id);
 Z   ALTER TABLE ONLY public."MathModelCoefficient" DROP CONSTRAINT "PK_MathModelCoefficient";
       public            postgres    false    247            i           2606    16610 6   MathModelCoefficientValue PK_MathModelCoefficientValue 
   CONSTRAINT     �   ALTER TABLE ONLY public."MathModelCoefficientValue"
    ADD CONSTRAINT "PK_MathModelCoefficientValue" PRIMARY KEY (id_model, id_coefficient, id_polymer);
 d   ALTER TABLE ONLY public."MathModelCoefficientValue" DROP CONSTRAINT "PK_MathModelCoefficientValue";
       public            postgres    false    248    248    248            k           2606    16612    Polymer PK_Polymer 
   CONSTRAINT     T   ALTER TABLE ONLY public."Polymer"
    ADD CONSTRAINT "PK_Polymer" PRIMARY KEY (id);
 @   ALTER TABLE ONLY public."Polymer" DROP CONSTRAINT "PK_Polymer";
       public            postgres    false    251            n           2606    16614 "   PolymerParametr PK_PolymerParametr 
   CONSTRAINT     d   ALTER TABLE ONLY public."PolymerParametr"
    ADD CONSTRAINT "PK_PolymerParametr" PRIMARY KEY (id);
 P   ALTER TABLE ONLY public."PolymerParametr" DROP CONSTRAINT "PK_PolymerParametr";
       public            postgres    false    252            p           2606    16616 ,   PolymerParametrValue PK_PolymerParametrValue 
   CONSTRAINT     �   ALTER TABLE ONLY public."PolymerParametrValue"
    ADD CONSTRAINT "PK_PolymerParametrValue" PRIMARY KEY (id_polymer, id_parametr);
 Z   ALTER TABLE ONLY public."PolymerParametrValue" DROP CONSTRAINT "PK_PolymerParametrValue";
       public            postgres    false    253    253            s           2606    16618 "   ProcessParametr PK_ProcessParametr 
   CONSTRAINT     d   ALTER TABLE ONLY public."ProcessParametr"
    ADD CONSTRAINT "PK_ProcessParametr" PRIMARY KEY (id);
 P   ALTER TABLE ONLY public."ProcessParametr" DROP CONSTRAINT "PK_ProcessParametr";
       public            postgres    false    256            u           2606    16620 ,   ProcessParametrValue PK_ProcessParametrValue 
   CONSTRAINT     �   ALTER TABLE ONLY public."ProcessParametrValue"
    ADD CONSTRAINT "PK_ProcessParametrValue" PRIMARY KEY (id_film, id_parametr);
 Z   ALTER TABLE ONLY public."ProcessParametrValue" DROP CONSTRAINT "PK_ProcessParametrValue";
       public            postgres    false    257    257            y           2606    16622    Scenario PK_Scenario 
   CONSTRAINT     V   ALTER TABLE ONLY public."Scenario"
    ADD CONSTRAINT "PK_Scenario" PRIMARY KEY (id);
 B   ALTER TABLE ONLY public."Scenario" DROP CONSTRAINT "PK_Scenario";
       public            postgres    false    259            {           2606    16624    Screw PK_Screw 
   CONSTRAINT     P   ALTER TABLE ONLY public."Screw"
    ADD CONSTRAINT "PK_Screw" PRIMARY KEY (id);
 <   ALTER TABLE ONLY public."Screw" DROP CONSTRAINT "PK_Screw";
       public            postgres    false    261            }           2606    16626    ScrewElement PK_ScrewElement 
   CONSTRAINT     ^   ALTER TABLE ONLY public."ScrewElement"
    ADD CONSTRAINT "PK_ScrewElement" PRIMARY KEY (id);
 J   ALTER TABLE ONLY public."ScrewElement" DROP CONSTRAINT "PK_ScrewElement";
       public            postgres    false    262            �           2606    16628 <   ScrewElementInСonfiguration PK_ScrewElementInСonfiguration 
   CONSTRAINT     ~   ALTER TABLE ONLY public."ScrewElementInСonfiguration"
    ADD CONSTRAINT "PK_ScrewElementInСonfiguration" PRIMARY KEY (id);
 j   ALTER TABLE ONLY public."ScrewElementInСonfiguration" DROP CONSTRAINT "PK_ScrewElementInСonfiguration";
       public            postgres    false    263            �           2606    16630 ,   ScrewElementParametr PK_ScrewElementParametr 
   CONSTRAINT     n   ALTER TABLE ONLY public."ScrewElementParametr"
    ADD CONSTRAINT "PK_ScrewElementParametr" PRIMARY KEY (id);
 Z   ALTER TABLE ONLY public."ScrewElementParametr" DROP CONSTRAINT "PK_ScrewElementParametr";
       public            postgres    false    265            �           2606    16632 6   ScrewElementParametrValue PK_ScrewElementParametrValue 
   CONSTRAINT     �   ALTER TABLE ONLY public."ScrewElementParametrValue"
    ADD CONSTRAINT "PK_ScrewElementParametrValue" PRIMARY KEY (id_parametr, id_element);
 d   ALTER TABLE ONLY public."ScrewElementParametrValue" DROP CONSTRAINT "PK_ScrewElementParametrValue";
       public            postgres    false    266    266            �           2606    16634    ScrewParametr PK_ScrewParametr 
   CONSTRAINT     `   ALTER TABLE ONLY public."ScrewParametr"
    ADD CONSTRAINT "PK_ScrewParametr" PRIMARY KEY (id);
 L   ALTER TABLE ONLY public."ScrewParametr" DROP CONSTRAINT "PK_ScrewParametr";
       public            postgres    false    269            �           2606    16636 (   ScrewParametrValue PK_ScrewParametrValue 
   CONSTRAINT     }   ALTER TABLE ONLY public."ScrewParametrValue"
    ADD CONSTRAINT "PK_ScrewParametrValue" PRIMARY KEY (id_screw, id_parametr);
 V   ALTER TABLE ONLY public."ScrewParametrValue" DROP CONSTRAINT "PK_ScrewParametrValue";
       public            postgres    false    270    270            �           2606    16638 :   ScrewPossibleСonfiguration PK_ScrewPossibleСonfiguration 
   CONSTRAINT     |   ALTER TABLE ONLY public."ScrewPossibleСonfiguration"
    ADD CONSTRAINT "PK_ScrewPossibleСonfiguration" PRIMARY KEY (id);
 h   ALTER TABLE ONLY public."ScrewPossibleСonfiguration" DROP CONSTRAINT "PK_ScrewPossibleСonfiguration";
       public            postgres    false    272            �           2606    16640 *   ScrewСonfiguration PK_ScrewСonfiguration 
   CONSTRAINT     l   ALTER TABLE ONLY public."ScrewСonfiguration"
    ADD CONSTRAINT "PK_ScrewСonfiguration" PRIMARY KEY (id);
 X   ALTER TABLE ONLY public."ScrewСonfiguration" DROP CONSTRAINT "PK_ScrewСonfiguration";
       public            postgres    false    275            �           2606    16642    Unit PK_Unit 
   CONSTRAINT     N   ALTER TABLE ONLY public."Unit"
    ADD CONSTRAINT "PK_Unit" PRIMARY KEY (id);
 :   ALTER TABLE ONLY public."Unit" DROP CONSTRAINT "PK_Unit";
       public            postgres    false    277            v           1259    16643    ExtruderIndex    INDEX     M   CREATE INDEX "ExtruderIndex" ON public."Scenario" USING btree (id_extruder);
 #   DROP INDEX public."ExtruderIndex";
       public            postgres    false    259            w           1259    16644 	   FilmIndex    INDEX     E   CREATE INDEX "FilmIndex" ON public."Scenario" USING btree (id_film);
    DROP INDEX public."FilmIndex";
       public            postgres    false    259            8           1259    16645    IX_Body    INDEX     W   CREATE INDEX "IX_Body" ON public."BarrelPossibleСonfiguration" USING btree (id_body);
    DROP INDEX public."IX_Body";
       public            postgres    false    219            9           1259    16646    IX_Configuration    INDEX     i   CREATE INDEX "IX_Configuration" ON public."BarrelPossibleСonfiguration" USING btree (id_configuration);
 &   DROP INDEX public."IX_Configuration";
       public            postgres    false    219            M           1259    16647    IX_Die    INDEX     S   CREATE INDEX "IX_Die" ON public."DieElementInСonfiguration" USING btree (id_die);
    DROP INDEX public."IX_Die";
       public            postgres    false    233            ~           1259    16648 
   IX_Element    INDEX     ]   CREATE INDEX "IX_Element" ON public."ScrewElementInСonfiguration" USING btree (id_element);
     DROP INDEX public."IX_Element";
       public            postgres    false    263            N           1259    16649    IX_Element111    INDEX     ^   CREATE INDEX "IX_Element111" ON public."DieElementInСonfiguration" USING btree (id_element);
 #   DROP INDEX public."IX_Element111";
       public            postgres    false    233            Q           1259    16650    IX_Parametr3    INDEX     R   CREATE INDEX "IX_Parametr3" ON public."DieElementParametr" USING btree (id_unit);
 "   DROP INDEX public."IX_Parametr3";
       public            postgres    false    235            e           1259    16651    IX_Relationship13    INDEX     Y   CREATE INDEX "IX_Relationship13" ON public."MathModelCoefficient" USING btree (id_unit);
 '   DROP INDEX public."IX_Relationship13";
       public            postgres    false    247            ]           1259    16652    IX_Relationship2    INDEX     Q   CREATE INDEX "IX_Relationship2" ON public."ExtruderType" USING btree (id_model);
 &   DROP INDEX public."IX_Relationship2";
       public            postgres    false    241            B           1259    16653    IX_Relationship21    INDEX     Z   CREATE INDEX "IX_Relationship21" ON public."BarrelSectionParametr" USING btree (id_unit);
 '   DROP INDEX public."IX_Relationship21";
       public            postgres    false    224            3           1259    16654    IX_Relationship211    INDEX     T   CREATE INDEX "IX_Relationship211" ON public."BarrelParametr" USING btree (id_unit);
 (   DROP INDEX public."IX_Relationship211";
       public            postgres    false    216            V           1259    16655    IX_Relationship23    INDEX     O   CREATE INDEX "IX_Relationship23" ON public."Extruder" USING btree (id_screw2);
 '   DROP INDEX public."IX_Relationship23";
       public            postgres    false    240            W           1259    16656    IX_Relationship24    INDEX     O   CREATE INDEX "IX_Relationship24" ON public."Extruder" USING btree (id_screw1);
 '   DROP INDEX public."IX_Relationship24";
       public            postgres    false    240            X           1259    16657    IX_Relationship25    INDEX     L   CREATE INDEX "IX_Relationship25" ON public."Extruder" USING btree (id_die);
 '   DROP INDEX public."IX_Relationship25";
       public            postgres    false    240            Y           1259    16658    IX_Relationship26    INDEX     O   CREATE INDEX "IX_Relationship26" ON public."Extruder" USING btree (id_barrel);
 '   DROP INDEX public."IX_Relationship26";
       public            postgres    false    240            Z           1259    16659    IX_Relationship3    INDEX     L   CREATE INDEX "IX_Relationship3" ON public."Extruder" USING btree (id_type);
 &   DROP INDEX public."IX_Relationship3";
       public            postgres    false    240            �           1259    16660    IX_Relationship34    INDEX     Y   CREATE INDEX "IX_Relationship34" ON public."ScrewElementParametr" USING btree (id_unit);
 '   DROP INDEX public."IX_Relationship34";
       public            postgres    false    265            �           1259    16661    IX_Relationship4    INDEX     Q   CREATE INDEX "IX_Relationship4" ON public."ScrewParametr" USING btree (id_unit);
 &   DROP INDEX public."IX_Relationship4";
       public            postgres    false    269            �           1259    16904    IX_Relationship401    INDEX     S   CREATE INDEX "IX_Relationship401" ON public."ColorInterval" USING btree (film_id);
 (   DROP INDEX public."IX_Relationship401";
       public            postgres    false    280            l           1259    16662    IX_Relationship5    INDEX     S   CREATE INDEX "IX_Relationship5" ON public."PolymerParametr" USING btree (id_unit);
 &   DROP INDEX public."IX_Relationship5";
       public            postgres    false    252            `           1259    16663    IX_Relationship6    INDEX     K   CREATE INDEX "IX_Relationship6" ON public."Film" USING btree (id_polymer);
 &   DROP INDEX public."IX_Relationship6";
       public            postgres    false    244            q           1259    16664    IX_Relationship7    INDEX     S   CREATE INDEX "IX_Relationship7" ON public."ProcessParametr" USING btree (id_unit);
 &   DROP INDEX public."IX_Relationship7";
       public            postgres    false    256            �           1259    16665    IX_Screw    INDEX     X   CREATE INDEX "IX_Screw" ON public."ScrewPossibleСonfiguration" USING btree (id_screw);
    DROP INDEX public."IX_Screw";
       public            postgres    false    272            >           1259    16666 
   IX_Section    INDEX     ^   CREATE INDEX "IX_Section" ON public."BarrelSectionInСonfiguration" USING btree (id_element);
     DROP INDEX public."IX_Section";
       public            postgres    false    222            ?           1259    16667    IX_Сonfiguration    INDEX     k   CREATE INDEX "IX_Сonfiguration" ON public."BarrelSectionInСonfiguration" USING btree (id_configuration);
 '   DROP INDEX public."IX_Сonfiguration";
       public            postgres    false    222                       1259    16668    IX_Сonfiguration11111    INDEX     o   CREATE INDEX "IX_Сonfiguration11111" ON public."ScrewElementInСonfiguration" USING btree (id_configuration);
 ,   DROP INDEX public."IX_Сonfiguration11111";
       public            postgres    false    263            �           1259    16669    IX_Сonfiguration22222    INDEX     n   CREATE INDEX "IX_Сonfiguration22222" ON public."ScrewPossibleСonfiguration" USING btree (id_configuration);
 ,   DROP INDEX public."IX_Сonfiguration22222";
       public            postgres    false    272            �           2606    16670 #   BarrelPossibleСonfiguration Barrel    FK CONSTRAINT     �   ALTER TABLE ONLY public."BarrelPossibleСonfiguration"
    ADD CONSTRAINT "Barrel" FOREIGN KEY (id_body) REFERENCES public."Barrel"(id);
 Q   ALTER TABLE ONLY public."BarrelPossibleСonfiguration" DROP CONSTRAINT "Barrel";
       public          postgres    false    219    215    3378            �           2606    16675    BarrelParametrValue Barrel    FK CONSTRAINT     �   ALTER TABLE ONLY public."BarrelParametrValue"
    ADD CONSTRAINT "Barrel" FOREIGN KEY (id_barrel) REFERENCES public."Barrel"(id);
 H   ALTER TABLE ONLY public."BarrelParametrValue" DROP CONSTRAINT "Barrel";
       public          postgres    false    3378    217    215            �           2606    16680    Extruder BodyConfiguration    FK CONSTRAINT     �   ALTER TABLE ONLY public."Extruder"
    ADD CONSTRAINT "BodyConfiguration" FOREIGN KEY (id_barrel) REFERENCES public."BarrelPossibleСonfiguration"(id);
 H   ALTER TABLE ONLY public."Extruder" DROP CONSTRAINT "BodyConfiguration";
       public          postgres    false    240    219    3387            �           2606    16685 *   BarrelPossibleСonfiguration Configuration    FK CONSTRAINT     �   ALTER TABLE ONLY public."BarrelPossibleСonfiguration"
    ADD CONSTRAINT "Configuration" FOREIGN KEY (id_configuration) REFERENCES public."BarrelСonfiguration"(id);
 X   ALTER TABLE ONLY public."BarrelPossibleСonfiguration" DROP CONSTRAINT "Configuration";
       public          postgres    false    229    3400    219            �           2606    16690    DieElementInСonfiguration Die    FK CONSTRAINT     �   ALTER TABLE ONLY public."DieElementInСonfiguration"
    ADD CONSTRAINT "Die" FOREIGN KEY (id_die) REFERENCES public."Die"(id);
 L   ALTER TABLE ONLY public."DieElementInСonfiguration" DROP CONSTRAINT "Die";
       public          postgres    false    231    233    3402            �           2606    16695 $   ScrewElementInСonfiguration Element    FK CONSTRAINT     �   ALTER TABLE ONLY public."ScrewElementInСonfiguration"
    ADD CONSTRAINT "Element" FOREIGN KEY (id_element) REFERENCES public."ScrewElement"(id);
 R   ALTER TABLE ONLY public."ScrewElementInСonfiguration" DROP CONSTRAINT "Element";
       public          postgres    false    263    262    3453            �           2606    16700 !   ScrewElementParametrValue Element    FK CONSTRAINT     �   ALTER TABLE ONLY public."ScrewElementParametrValue"
    ADD CONSTRAINT "Element" FOREIGN KEY (id_element) REFERENCES public."ScrewElement"(id);
 O   ALTER TABLE ONLY public."ScrewElementParametrValue" DROP CONSTRAINT "Element";
       public          postgres    false    262    3453    266            �           2606    16705    DieElementParametrValue Element    FK CONSTRAINT     �   ALTER TABLE ONLY public."DieElementParametrValue"
    ADD CONSTRAINT "Element" FOREIGN KEY (id_element) REFERENCES public."DieElement"(id);
 M   ALTER TABLE ONLY public."DieElementParametrValue" DROP CONSTRAINT "Element";
       public          postgres    false    236    232    3404            �           2606    16710 "   DieElementInСonfiguration Element    FK CONSTRAINT     �   ALTER TABLE ONLY public."DieElementInСonfiguration"
    ADD CONSTRAINT "Element" FOREIGN KEY (id_element) REFERENCES public."DieElement"(id);
 P   ALTER TABLE ONLY public."DieElementInСonfiguration" DROP CONSTRAINT "Element";
       public          postgres    false    232    3404    233            �           2606    16715    Scenario Extruder    FK CONSTRAINT     }   ALTER TABLE ONLY public."Scenario"
    ADD CONSTRAINT "Extruder" FOREIGN KEY (id_extruder) REFERENCES public."Extruder"(id);
 ?   ALTER TABLE ONLY public."Scenario" DROP CONSTRAINT "Extruder";
       public          postgres    false    259    240    3420            �           2606    16720    ProcessParametrValue Film    FK CONSTRAINT     }   ALTER TABLE ONLY public."ProcessParametrValue"
    ADD CONSTRAINT "Film" FOREIGN KEY (id_film) REFERENCES public."Film"(id);
 G   ALTER TABLE ONLY public."ProcessParametrValue" DROP CONSTRAINT "Film";
       public          postgres    false    244    3426    257            �           2606    16725    Scenario Film    FK CONSTRAINT     q   ALTER TABLE ONLY public."Scenario"
    ADD CONSTRAINT "Film" FOREIGN KEY (id_film) REFERENCES public."Film"(id);
 ;   ALTER TABLE ONLY public."Scenario" DROP CONSTRAINT "Film";
       public          postgres    false    244    3426    259            �           2606    16899    ColorInterval Film    FK CONSTRAINT     �   ALTER TABLE ONLY public."ColorInterval"
    ADD CONSTRAINT "Film" FOREIGN KEY (film_id) REFERENCES public."Film"(id) ON DELETE CASCADE;
 @   ALTER TABLE ONLY public."ColorInterval" DROP CONSTRAINT "Film";
       public          postgres    false    280    244    3426            �           2606    16730    Extruder HeadConfiguration    FK CONSTRAINT     |   ALTER TABLE ONLY public."Extruder"
    ADD CONSTRAINT "HeadConfiguration" FOREIGN KEY (id_die) REFERENCES public."Die"(id);
 H   ALTER TABLE ONLY public."Extruder" DROP CONSTRAINT "HeadConfiguration";
       public          postgres    false    231    240    3402            �           2606    16735 #   MathModelCoefficientValue MathModel    FK CONSTRAINT     �   ALTER TABLE ONLY public."MathModelCoefficientValue"
    ADD CONSTRAINT "MathModel" FOREIGN KEY (id_model) REFERENCES public."MathModel"(id);
 Q   ALTER TABLE ONLY public."MathModelCoefficientValue" DROP CONSTRAINT "MathModel";
       public          postgres    false    246    3428    248            �           2606    16740    ExtruderType MathModel    FK CONSTRAINT     �   ALTER TABLE ONLY public."ExtruderType"
    ADD CONSTRAINT "MathModel" FOREIGN KEY (id_model) REFERENCES public."MathModel"(id);
 D   ALTER TABLE ONLY public."ExtruderType" DROP CONSTRAINT "MathModel";
       public          postgres    false    3428    246    241            �           2606    16745    PolymerParametrValue Parametr    FK CONSTRAINT     �   ALTER TABLE ONLY public."PolymerParametrValue"
    ADD CONSTRAINT "Parametr" FOREIGN KEY (id_parametr) REFERENCES public."PolymerParametr"(id);
 K   ALTER TABLE ONLY public."PolymerParametrValue" DROP CONSTRAINT "Parametr";
       public          postgres    false    3438    252    253            �           2606    16750    ScrewParametrValue Parametr    FK CONSTRAINT     �   ALTER TABLE ONLY public."ScrewParametrValue"
    ADD CONSTRAINT "Parametr" FOREIGN KEY (id_parametr) REFERENCES public."ScrewParametr"(id);
 I   ALTER TABLE ONLY public."ScrewParametrValue" DROP CONSTRAINT "Parametr";
       public          postgres    false    270    269    3465            �           2606    16755 "   ScrewElementParametrValue Parametr    FK CONSTRAINT     �   ALTER TABLE ONLY public."ScrewElementParametrValue"
    ADD CONSTRAINT "Parametr" FOREIGN KEY (id_parametr) REFERENCES public."ScrewElementParametr"(id);
 P   ALTER TABLE ONLY public."ScrewElementParametrValue" DROP CONSTRAINT "Parametr";
       public          postgres    false    3460    266    265            �           2606    16760    ProcessParametrValue Parametr    FK CONSTRAINT     �   ALTER TABLE ONLY public."ProcessParametrValue"
    ADD CONSTRAINT "Parametr" FOREIGN KEY (id_parametr) REFERENCES public."ProcessParametr"(id);
 K   ALTER TABLE ONLY public."ProcessParametrValue" DROP CONSTRAINT "Parametr";
       public          postgres    false    256    3443    257            �           2606    16765 "   MathModelCoefficientValue Parametr    FK CONSTRAINT     �   ALTER TABLE ONLY public."MathModelCoefficientValue"
    ADD CONSTRAINT "Parametr" FOREIGN KEY (id_coefficient) REFERENCES public."MathModelCoefficient"(id);
 P   ALTER TABLE ONLY public."MathModelCoefficientValue" DROP CONSTRAINT "Parametr";
       public          postgres    false    248    3431    247            �           2606    16770     DieElementParametrValue Parametr    FK CONSTRAINT     �   ALTER TABLE ONLY public."DieElementParametrValue"
    ADD CONSTRAINT "Parametr" FOREIGN KEY (id_parametr) REFERENCES public."DieElementParametr"(id);
 N   ALTER TABLE ONLY public."DieElementParametrValue" DROP CONSTRAINT "Parametr";
       public          postgres    false    236    3411    235            �           2606    16775    BarrelParametrValue Parametr    FK CONSTRAINT     �   ALTER TABLE ONLY public."BarrelParametrValue"
    ADD CONSTRAINT "Parametr" FOREIGN KEY (id_parametr) REFERENCES public."BarrelParametr"(id);
 J   ALTER TABLE ONLY public."BarrelParametrValue" DROP CONSTRAINT "Parametr";
       public          postgres    false    217    3381    216            �           2606    16780 #   BarrelSectionParametrValue Parametr    FK CONSTRAINT     �   ALTER TABLE ONLY public."BarrelSectionParametrValue"
    ADD CONSTRAINT "Parametr" FOREIGN KEY (id_parametr) REFERENCES public."BarrelSectionParametr"(id);
 Q   ALTER TABLE ONLY public."BarrelSectionParametrValue" DROP CONSTRAINT "Parametr";
       public          postgres    false    224    3396    225            �           2606    16785    PolymerParametrValue Polymer    FK CONSTRAINT     �   ALTER TABLE ONLY public."PolymerParametrValue"
    ADD CONSTRAINT "Polymer" FOREIGN KEY (id_polymer) REFERENCES public."Polymer"(id);
 J   ALTER TABLE ONLY public."PolymerParametrValue" DROP CONSTRAINT "Polymer";
       public          postgres    false    253    3435    251            �           2606    16790    Film Polymer    FK CONSTRAINT     v   ALTER TABLE ONLY public."Film"
    ADD CONSTRAINT "Polymer" FOREIGN KEY (id_polymer) REFERENCES public."Polymer"(id);
 :   ALTER TABLE ONLY public."Film" DROP CONSTRAINT "Polymer";
       public          postgres    false    244    3435    251            �           2606    16795 !   MathModelCoefficientValue Polymer    FK CONSTRAINT     �   ALTER TABLE ONLY public."MathModelCoefficientValue"
    ADD CONSTRAINT "Polymer" FOREIGN KEY (id_polymer) REFERENCES public."Polymer"(id);
 O   ALTER TABLE ONLY public."MathModelCoefficientValue" DROP CONSTRAINT "Polymer";
       public          postgres    false    248    3435    251            �           2606    16800    Extruder ScreConfiguration    FK CONSTRAINT     �   ALTER TABLE ONLY public."Extruder"
    ADD CONSTRAINT "ScreConfiguration" FOREIGN KEY (id_screw1) REFERENCES public."ScrewPossibleСonfiguration"(id);
 H   ALTER TABLE ONLY public."Extruder" DROP CONSTRAINT "ScreConfiguration";
       public          postgres    false    240    3471    272            �           2606    16805 !   ScrewPossibleСonfiguration Screw    FK CONSTRAINT     �   ALTER TABLE ONLY public."ScrewPossibleСonfiguration"
    ADD CONSTRAINT "Screw" FOREIGN KEY (id_screw) REFERENCES public."Screw"(id);
 O   ALTER TABLE ONLY public."ScrewPossibleСonfiguration" DROP CONSTRAINT "Screw";
       public          postgres    false    272    3451    261            �           2606    16810    ScrewParametrValue Screw    FK CONSTRAINT     ~   ALTER TABLE ONLY public."ScrewParametrValue"
    ADD CONSTRAINT "Screw" FOREIGN KEY (id_screw) REFERENCES public."Screw"(id);
 F   ALTER TABLE ONLY public."ScrewParametrValue" DROP CONSTRAINT "Screw";
       public          postgres    false    270    3451    261            �           2606    16815    Extruder ScrewConfiguration    FK CONSTRAINT     �   ALTER TABLE ONLY public."Extruder"
    ADD CONSTRAINT "ScrewConfiguration" FOREIGN KEY (id_screw2) REFERENCES public."ScrewPossibleСonfiguration"(id);
 I   ALTER TABLE ONLY public."Extruder" DROP CONSTRAINT "ScrewConfiguration";
       public          postgres    false    3471    240    272            �           2606    16820 %   BarrelSectionInСonfiguration Section    FK CONSTRAINT     �   ALTER TABLE ONLY public."BarrelSectionInСonfiguration"
    ADD CONSTRAINT "Section" FOREIGN KEY (id_element) REFERENCES public."BarrelSection"(id);
 S   ALTER TABLE ONLY public."BarrelSectionInСonfiguration" DROP CONSTRAINT "Section";
       public          postgres    false    3389    222    221            �           2606    16825 "   BarrelSectionParametrValue Section    FK CONSTRAINT     �   ALTER TABLE ONLY public."BarrelSectionParametrValue"
    ADD CONSTRAINT "Section" FOREIGN KEY (id_element) REFERENCES public."BarrelSection"(id);
 P   ALTER TABLE ONLY public."BarrelSectionParametrValue" DROP CONSTRAINT "Section";
       public          postgres    false    221    225    3389            �           2606    16830    Extruder Type    FK CONSTRAINT     y   ALTER TABLE ONLY public."Extruder"
    ADD CONSTRAINT "Type" FOREIGN KEY (id_type) REFERENCES public."ExtruderType"(id);
 ;   ALTER TABLE ONLY public."Extruder" DROP CONSTRAINT "Type";
       public          postgres    false    3423    240    241            �           2606    16835    ScrewElementParametr Unit    FK CONSTRAINT     }   ALTER TABLE ONLY public."ScrewElementParametr"
    ADD CONSTRAINT "Unit" FOREIGN KEY (id_unit) REFERENCES public."Unit"(id);
 G   ALTER TABLE ONLY public."ScrewElementParametr" DROP CONSTRAINT "Unit";
       public          postgres    false    277    265    3475            �           2606    16840    ScrewParametr Unit    FK CONSTRAINT     v   ALTER TABLE ONLY public."ScrewParametr"
    ADD CONSTRAINT "Unit" FOREIGN KEY (id_unit) REFERENCES public."Unit"(id);
 @   ALTER TABLE ONLY public."ScrewParametr" DROP CONSTRAINT "Unit";
       public          postgres    false    277    269    3475            �           2606    16845    PolymerParametr Unit    FK CONSTRAINT     x   ALTER TABLE ONLY public."PolymerParametr"
    ADD CONSTRAINT "Unit" FOREIGN KEY (id_unit) REFERENCES public."Unit"(id);
 B   ALTER TABLE ONLY public."PolymerParametr" DROP CONSTRAINT "Unit";
       public          postgres    false    277    3475    252            �           2606    16850    ProcessParametr Unit    FK CONSTRAINT     x   ALTER TABLE ONLY public."ProcessParametr"
    ADD CONSTRAINT "Unit" FOREIGN KEY (id_unit) REFERENCES public."Unit"(id);
 B   ALTER TABLE ONLY public."ProcessParametr" DROP CONSTRAINT "Unit";
       public          postgres    false    3475    277    256            �           2606    16855    MathModelCoefficient Unit    FK CONSTRAINT     }   ALTER TABLE ONLY public."MathModelCoefficient"
    ADD CONSTRAINT "Unit" FOREIGN KEY (id_unit) REFERENCES public."Unit"(id);
 G   ALTER TABLE ONLY public."MathModelCoefficient" DROP CONSTRAINT "Unit";
       public          postgres    false    3475    247    277            �           2606    16860    DieElementParametr Unit    FK CONSTRAINT     {   ALTER TABLE ONLY public."DieElementParametr"
    ADD CONSTRAINT "Unit" FOREIGN KEY (id_unit) REFERENCES public."Unit"(id);
 E   ALTER TABLE ONLY public."DieElementParametr" DROP CONSTRAINT "Unit";
       public          postgres    false    277    3475    235            �           2606    16865    BarrelParametr Unit    FK CONSTRAINT     w   ALTER TABLE ONLY public."BarrelParametr"
    ADD CONSTRAINT "Unit" FOREIGN KEY (id_unit) REFERENCES public."Unit"(id);
 A   ALTER TABLE ONLY public."BarrelParametr" DROP CONSTRAINT "Unit";
       public          postgres    false    277    216    3475            �           2606    16870    BarrelSectionParametr Unit    FK CONSTRAINT     ~   ALTER TABLE ONLY public."BarrelSectionParametr"
    ADD CONSTRAINT "Unit" FOREIGN KEY (id_unit) REFERENCES public."Unit"(id);
 H   ALTER TABLE ONLY public."BarrelSectionParametr" DROP CONSTRAINT "Unit";
       public          postgres    false    3475    224    277            �           2606    16875 +   ScrewElementInСonfiguration Сonfiguration    FK CONSTRAINT     �   ALTER TABLE ONLY public."ScrewElementInСonfiguration"
    ADD CONSTRAINT "Сonfiguration" FOREIGN KEY (id_configuration) REFERENCES public."ScrewСonfiguration"(id);
 Y   ALTER TABLE ONLY public."ScrewElementInСonfiguration" DROP CONSTRAINT "Сonfiguration";
       public          postgres    false    263    3473    275            �           2606    16880 *   ScrewPossibleСonfiguration Сonfiguration    FK CONSTRAINT     �   ALTER TABLE ONLY public."ScrewPossibleСonfiguration"
    ADD CONSTRAINT "Сonfiguration" FOREIGN KEY (id_configuration) REFERENCES public."ScrewСonfiguration"(id);
 X   ALTER TABLE ONLY public."ScrewPossibleСonfiguration" DROP CONSTRAINT "Сonfiguration";
       public          postgres    false    272    275    3473            �           2606    16885 ,   BarrelSectionInСonfiguration Сonfiguration    FK CONSTRAINT     �   ALTER TABLE ONLY public."BarrelSectionInСonfiguration"
    ADD CONSTRAINT "Сonfiguration" FOREIGN KEY (id_configuration) REFERENCES public."BarrelСonfiguration"(id);
 Z   ALTER TABLE ONLY public."BarrelSectionInСonfiguration" DROP CONSTRAINT "Сonfiguration";
       public          postgres    false    229    222    3400            S       x�3�tJ,*J�1�2����̠,c�=... �	      T      x�3�4��I̍��t������� 6s�      U      x�3�4�44�3�2�1�`�=... K��      W      x�3�4�4�2�B.3N ����� "{�      Y   6   x�3�t�I�M�+1�23��L`BFF\�p�1�1�m�ecr���F\1z\\\ �_1      Z   7   x���	 0�u��I�ڥ��QÁ�)�a8A&�Bӣ�,�L�`_׵}�A�_I      \   M   x�3�4�K,��t�2�}�S��lc �%5'>;�1rB��!S �1�3�as���0�����qqq M      ]   �   x�MPI�0;�_�l �K���*�t&ɒ���������M<�qc���I���s���9Q�ȴ}�邝6L{�(��ď�}Yc�����ua`��?��}�6s'�9�ʣ�]�*��3Ƽ��іw[��۴�����?R��������4���f��f�H��ܠF��L?��� ա4�      a   )   x�3�tJ,*J�q��K�L/-J,����2�*j�U4F��� �p�      �   {   x�M���1��d� Cx�]z�?A�WCz�@���J��PGf.�	���`_��@�]?\�,^�M۲��������@ֽԜ��DB"�
X�C�#�<�`�Z�f������ܗx���&"�      c   (   x�3�t��+�I,r�L�D�qp�楀db���� 3P�      d   D   x�3�t�Lu�I�5�2�1�LaLc.3ӄ��֐��6Ⲅ����.CC'5��ؐ+F��� ��      e   A   x��� 1�3���zI�u,��px�D@L��"�`����6PyM�k���h��%����ȣ
�      g   P   x�3�4���s��t�2�\�K�Lc03-3�6�}�S�A�@ND����\���
䙃u8A�,��1z\\\ �3      h   �   x�5�K�!CǸ�>��{y�_G'��p�$}�����n>�V�Ľ;DZ�\\��m�%iɖ�u���#�+��gc�$�E�EpZ�S@�Ĵ"��ኖT74�$[�LD�2��T(	�]P.��Jm�&T$�c˶;�n&7ڶ;�M��|�}��D&#�PR�F8�����o��`۱��E����p}<���w-��������c�A�D�      l   G   x�3�4�4�? ������\�ZniPah�e	��4ɚ��q�����f��F@�\1z\\\ �A�      m      x�3�4�,�,H����� ��      p   3   x���4�t��Oq���5�44�2�4�p�qp�rd$�BD�b���� �UR      r      x�3���OI������ ��      s      x������ � �      t      x������ � �      w   '   x�3��ϩL-ɨ�I�K�2A�q�r�9s��qqq .��      x   l   x�-λ
�0��}����ं�����V���{Һ}?!$J(ah$QI��5�'p�z��3�	e��ȁ��/:��N
*w���+򵝮��4<0c�7S �� Ec!      y   �   x�5��!�c0����\�q܀��iiKǀ5��>�{fdN����\�P��tS!�0wY���7�%c9��8x���#{䩞�x<�<�c�Nk�*�H
�ّ���7��P��@��*�5S�;��x@�h��Z�r�-�      |   '   x�3�4�t+��M��t�2r<S�c�� Ȋ���� �,�      }   <   x�3�4���4�2�41L�,c������L��G�U�T�B؆f�&\1z\\\ 4�         3   x�3�44�4�NN�3�41�42�2Gs ��)T��(���� `�
      �      x�3�N.J-7�2�0���!c�=... ��      �   J   x�Uɻ�0 њa;�`�t�P $֧�S�;����o;�4�X�(�c�2%s��ׄ���C��j���/_      �   A   x���� ��a���]����g%w��T��R�I8��'|�~�x96 ������>���E
�      �   �   x�%���0E��c����C&@bA���HU�H��=��v���#w�Ǻ����̹ ����8p�Q-��7�zӁo��q�!�CLz��"�i��Md\�I�'�ˠ��_���?�ƭŎ�g����{�f�+K      �   �  x�=�A�$!��a�	��]����Ŀ!��4m�!C>Ul�
��CGT,���>v֍zs�1V}�9D+e�
�'_�WҔl}o�;�Z:K+tB��μ�*��]G��~�,��SAV���.���)�
���Mզ�[��Q3ku�(<�P�0����-,��z#6ra#Wm���вAb$I��Ĉ2�r�ƞ�^�X���cX�UJ'6I?'Љw�fGN�S�q��&���t@��a�M�'�6M��y���>i�Y�:sE�2����xzF������jB~;Ǘ�Bz���� ���z�"^�o.Q��`'�� ? ��ۍ?$AI�lJ�%ђx̺���館#q��r�L`.�ֿ��{>y�g���=�D�:�܃/g��"���������      �   f   x�3�4���t�22|s㋁l3 ;,�(>�8��5rC�!l �%5���v̉s���C '1>�4��9�0E&@N^�g^Ij:��=... � .      �   }   x�E��� �v/�X"@��}�g���cIZl�슔� �����2�D�"`�?1diтS�	�1�)yX�E�D�EĐ����|�+mx�+�(�j�5gb�z
���yAW�U��os�Ɩ#      �      x�3�4�4�2�B.cN ����� !��      �   (   x�3�N.J-w��K�L/-J,����2�&h�M0F��� G��      �      x�3�L�-(������ j     